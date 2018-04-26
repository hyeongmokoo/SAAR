UncernSAM.mod <- function(x.e, x.v, nb)
{
  n <- length(x.e)
  
  res.bd <- vector(mode = 'numeric', length = n)
  res.bc <- vector(mode = 'numeric', length = n)
  res.hd <- vector(mode = 'numeric', length = n)
  
  nb.n <- card(nb)
  
  for(i in 1:n){
    tmp.bd <- 0
    tmp.bc <- 0
    tmp.hd <- 0
    
    for (j in 1:nb.n[i])
    {
      nb.idx <- nb[[i]][j]
      lc.bd <- Bhatta.dist(x.e[i], x.e[nb.idx], x.v[i], x.v[nb.idx])
      lc.bc <- exp(lc.bd * -1)
      lc.hd <- sqrt(1-lc.bc)
      
      tmp.bd <- tmp.bd + lc.bd
      tmp.bc <- tmp.bc + lc.bc
      tmp.hd <- tmp.hd + lc.hd
    }
    
    res.bd[i]<-tmp.bd/nb.n[i]
    res.bc[i]<-tmp.bc/nb.n[i]
    res.hd[i]<-tmp.hd/nb.n[i]
  }
  global.res <- c(mean(res.bd), mean(res.bc), mean(res.hd))
  names(global.res) <- c("bd", "bc", "hd")
  res <- list(global.res, res.bd, res.bc, res.hd)
  names(res) <- c("global", "local.bd", "local.bc", "local.hd")
  res
}


UncernSAM <- function(x.e, x.v, nb)
{
  n <- length(x.e)
  
  res.bd <- vector(mode = 'numeric', length = n)
  res.bc <- vector(mode = 'numeric', length = n)
  res.hd <- vector(mode = 'numeric', length = n)
  
  nb.n <- card(nb)
  
  for(i in 1:n){
    tmp.bd <- 0
    tmp.bc <- 0
    tmp.hd <- 0
    
    for (j in 1:nb.n[i])
    {
      nb.idx <- nb[[i]][j]
      lc.bd <- Bhatta.dist(x.e[i], x.e[nb.idx], x.v[i], x.v[nb.idx])
      lc.bc <- Bhatta.coef(x.e[i], x.e[nb.idx], x.v[i], x.v[nb.idx])
      lc.hd <- He.dist(x.e[i], x.e[nb.idx], x.v[i], x.v[nb.idx])
      
      tmp.bd <- tmp.bd + lc.bd
      tmp.bc <- tmp.bc + lc.bc
      tmp.hd <- tmp.hd + lc.hd
    }
    
    res.bd[i]<-tmp.bd/nb.n[i]
    res.bc[i]<-tmp.bc/nb.n[i]
    res.hd[i]<-tmp.hd/nb.n[i]
  }
  global.res <- c(mean(res.bd), mean(res.bc), mean(res.hd))
  names(global.res) <- c("bd", "bc", "hd")
  res <- list(global.res, res.bd, res.bc, res.hd)
  names(res) <- c("global", "local.bd", "local.bc", "local.hd")
  res
}



Bhatta.dist <- function(x1, x2, v1, v2){
  
  var.comp <- log((1/4)*((v1^2/v2^2)+(v2^2/v1^2)+2))
  mean.var.comp <- ((x1-x2)^2)/(v1^2+v2^2)
  bha.dist <- (1/4)*(var.comp+mean.var.comp)
  
  return(bha.dist)
}
Bhatta.coef <- function(x1, x2, v1, v2){
  
  var.comp <- log((1/4)*((v1^2/v2^2)+(v2^2/v1^2)+2))
  mean.var.comp <- ((x1-x2)^2)/(v1^2+v2^2)
  bha.dist <- (1/4)*(var.comp+mean.var.comp)
  bha.coef <- exp(bha.dist * -1)
  return(bha.coef)
}

He.dist <- function(x1, x2, v1, v2){
  
  var.comp <- log((1/4)*((v1^2/v2^2)+(v2^2/v1^2)+2))
  mean.var.comp <- ((x1-x2)^2)/(v1^2+v2^2)
  bha.dist <- (1/4)*(var.comp+mean.var.comp)
  bha.coef <- exp(bha.dist * -1)
  hell.dist <- sqrt(1-bha.coef)
  return(hell.dist)
}



test.local.geary <- function (x, id, nb, style="W", sig.levels=c(0.05), method="total", alternative="two.sided", diag.zero=TRUE){
  # purpose: Calculating local Geary's ci and conducting significance testing
  # Arguments:
  #   x: a vector of one variable
  #   id: a vector of ID codes
  #   nb: nb object
  #   sig: a vector of significance levels	
  #   style: style of spatial weight matrix (refer to nb2listw)
  #   method: "conditional" or "total" randomization, or "normality" assumption for null hypothesis
  #   alternatie: type of test
  #   diag.zero: if FALSE, each region has itself as its neighbour
  # Fuction called:
  #   nb.self
  
  alternative <- match.arg(alternative, c("greater", "less", "two.sided"))
  style <- match.arg(style, c("B","C","S","W","U","V"))
  method <- match.arg(method, c("conditional", "total", "normality"))
  mf <- match.call(expand.dots = FALSE)
  
  n <- length(x)
  
  # making non-zero diagonal elements in a spatial weight matrix
  if (style=="V") diag.zero <- FALSE
  if (diag.zero == FALSE) nb <- nb.self(nb)
  if (style=="V") {listw <- nb2listv(nb,beta=0.5)}
  else {listw <- nb2listw(nb,style=style)}
  
  row.sum <- unlist(lapply(listw$weights,sum))
  sum.V <- sum(row.sum)
  
  # dealing with diagonal elements
  vii.vec <- rep(0,n)
  if (diag.zero == FALSE) {
    iis <- unlist(lapply(1:n, function (x,nbs) match(x,nbs[[x]]), nbs=listw$neighbours))
    for (i in 1:n) vii.vec[i] <- listw$weights[[i]][iis[i]]
  }
  
  # some matrices for normality assumtion
  V <- as(as(as_dgRMatrix_listw(listw), "RsparseMatrix"), "CsparseMatrix")
  one.vec <- rep(1,n)
  M <- diag(n)-(one.vec%*%solve(crossprod(one.vec))%*%t(one.vec))
  
  # some basic calculations
  mu.x <- mean(x)
  sd.x <- sqrt(sum((x - mu.x)^2)/n)
  z.x <- (x-mu.x)/sd.x
  z.x.2 <- z.x^2
  
  K <- (n-1)/(2*sum.V)
  
  m2 <- sum((x-mean(x))^2)/n
  m3 <- sum((x-mean(x))^3)/n
  m4 <- sum((x-mean(x))^4)/n
  b1 <- m3/(m2^(3/2))
  b2 <- m4/m2^2
  
  # objects to store results
  
  local.geary.vec <- vector()
  E.C.vec <- vector()
  VAR.C.vec <- vector()
  #   v.z.x.vec <- vector()	
  # For each observation
  for (i in 1:n){
    vi.vec <- listw$weights[[i]]
    vi.nhbr.vec <- listw$neighbours[[i]]
    vi.s <- sum(vi.vec)
    vi.s.2 <- vi.s^2
    vi.2.s <- sum(vi.vec^2)
    vii <- vii.vec[i]
    vii.2 <- vii^2
    z.x.i <- z.x[i]
    z.x.j <- z.x[vi.nhbr.vec]
    #		v.z.x <- sum(z.x.j*vi.vec)
    # calculating a local L value
    local.geary.i <- K*sum(((z.x.i-z.x.j)^2)*vi.vec)
    local.geary.vec[i] <- local.geary.i
    
    # calculating an expectation and variance
    if (method=="conditional") {
      diag.value <- K*(vi.s-vii)*(z.x.i^2)
      E.C.off <- 0
      E.C.on <- (K/(n-1))*(vi.s-vii)*(n+z.x.i^2)
      E.C <- E.C.off + E.C.on
      VAR.C.off <- 0
      VAR.C.on <- (K^2/(((n-1)^2)*(n-2)))*(((n-1)*(vi.2.s-vii.2))-(vi.s-vii)^2)*(((n-1)*((n*b2)-(4*n*b1*z.x.i)+(4*n*z.x.i^2)-(z.x.i^4)))-(n+(z.x.i^2))^2)
      COV.C <- 0
      COV.C.2 <- 2*COV.C
      VAR.C <- VAR.C.off + VAR.C.on + COV.C.2
      E.C <- E.C + diag.value
    }
    else if (method=="total") {
      E.C.off <- K*(2/(n-1))*(vi.s-vii)
      E.C.on <- K*2*(vi.s-vii)
      E.C <- E.C.off + E.C.on
      VAR.C <- (K^2)*(n/(n-1))*((((vi.2.s-vii.2)+((vi.s-vii)^2))*(b2+3))-(4*((vi.s-vii)^2)*n/(n-1)))
    }
    else if (method=="normality") {
      Omega.i <- matrix(rep(0, n^2), ncol=n)
      V.i <- matrix(rep(0, n^2), ncol=n)
      V.i[i,] <- V[i,]
      diag(Omega.i) <- V[i,]
      Omega.i[i,i] <- Omega.i[i,i]+row.sum[i]
      Local.M <- Omega.i - V.i - t(V.i)
      T.i <- n*(n-1)*Local.M/(2*sum(V))
      T.i <- 0.5*(T.i+t(T.i))
      K.i <- M%*%T.i
      v.z.x <- sum(V.i%*%z.x)
      #		v.z.x.vec[i] <- v.z.x
      tr <- function (a)
      {
        sum(diag(a))
      }
      mu.1 <- tr(K.i)/(n-1)
      mu.2 <- 2*(((n-1)*tr(K.i%*%K.i))-(tr(K.i)^2))/(((n-1)^2)*(n+1))
      mu.3 <- 8*(((n-1)^2*tr(K.i%*%K.i%*%K.i))-(3*(n-1)*tr(K)*tr(K.i%*%K.i))+(2*(tr(K.i)^3)))/((n-1)^3*(n+1)*(n+3))
      mu.4 <- 12*((((n-1)^3)*(4*tr(K.i%*%K.i%*%K.i%*%K.i)+(tr(K.i%*%K.i)^2)))-((2*((n-1)^2))*((8*tr(K.i)*tr(K.i%*%K.i%*%K.i))+(tr(K.i%*%K.i)*(tr(K.i)^2))))+((n-1)*((24*tr(K.i%*%K.i)*(tr(K.i)^2))+(tr(K.i)^4)))-(12*(tr(K.i)^4)))/(((n-1)^4)*(n+1)*(n+3)*(n+5))
      E.C <- mu.1
      VAR.C <- mu.2
      SKEW.C <- mu.3/((mu.2)^(3/2))
      KURT.C <- mu.4/((mu.2)^2)
    }
    
    # store values
    E.C.vec[i] <- E.C
    VAR.C.vec[i] <- VAR.C
  }
  
  SD.C.vec <- sqrt(VAR.C.vec)
  Z.C.vec <- (local.geary.vec-E.C.vec)/SD.C.vec
  
  # calculating p-values
  #P.C.vec <- numeric(n)
  if (alternative=="two.sided") {
    P.C.vec <- sapply(Z.C.vec, function(x) {2 * pnorm(abs(x), lower.tail = FALSE)})}
  else if (alternative=="greater") {
    P.C.vec <- sapply(Z.C.vec, function(x) {pnorm(x, lower.tail = FALSE)})}
  else {
    P.C.vec <- sapply(Z.C.vec, function(x) {pnorm(x, lower.tail = TRUE)})}
  
  # an analogy of Moran scatterplot
  class.vec <- vector()
  for (q in 1:n)
  {
    if (z.x[q] < 0)
      class.vec[q] <- as.character("Low")
    else class.vec[q] <- as.character("High")
  }
  
  # Significance for different levels
  sig.df <- as.data.frame(sapply(sig.levels, function(x, p.l, c.d) {ifelse(p.l > x, "not sig.", c.d)}, p.l=P.C.vec, c.d=class.vec))
  colnames(sig.df) <- paste("sig", sig.levels, sep="")
  
  # To make a composite vector of quad and the order of significance
  n.sig <- length(sig.levels)
  sig.1 <- rev(sort(sig.levels))[1]
  final.quad.sig <- ifelse (P.C.vec <= sig.1, paste(class.vec, n.sig, sep=""), as.character("not sig."))
  k <- 2
  while (k <= n.sig)
  {
    sig.k <- rev(sort(sig.levels))[k]
    final.quad.sig[P.C.vec <= sig.k] <- paste(class.vec[P.C.vec <= sig.k], n.sig+1-k, sep="")
    k <- k+1
  }
  #	
  local.Geary <- data.frame(id=id, local.Geary=local.geary.vec, z.x=z.x, E=E.C.vec, VAR=VAR.C.vec,
                            SD=SD.C.vec, Z=Z.C.vec, P=P.C.vec, quad=class.vec, sig.df, final.quad=final.quad.sig)
  return(local.Geary)
}

