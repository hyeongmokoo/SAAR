##Functions for Clustogram
S.local.test.mod <- function (x, listw, method="conditional", alternative="two.sided", diag.zero=FALSE){
  # purpose: Calculate local Lee's L and test statistic under conditional or total randomization
  # Arguments:
  #   x: a vector of one variable
  #   id: a sequence of 1:n (n is the number of observations)
  #   nb: nb object
  #   sig: a vector of significance levels	
  #   style: style of spatial weight matrix (refer to nb2listw)
  #   method: "conditional" or "total" randomization, or "normality" assumption for null hypothesis
  #   alternatie: type of test
  #   diag.zero: if FALSE, each region has itself as its neighbour
  # Fuction called:
  #   nb.self
  sig.levels <- c(0.05)
  alternative <- match.arg(alternative, c("greater", "less", "two.sided"))
  style <- "W"
  method <- match.arg(method, c("conditional", "total", "normality"))
  mf <- match.call(expand.dots = FALSE)
  
  n <- length(x)
  id <- 1:n
  # making non-zero diagonal elements in a spatial weight matrix
  #if (diag.zero == FALSE) nb <- nb.self(nb)
  #listw <- nb2listw(nb,style=style)
  
  row.sum <- unlist(lapply(listw$weights,sum))
  sum.VtV <- sum(row.sum^2)
  
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
  
  K <- n/sum.VtV
  
  m2 <- sum((x-mu.x)^2)/n
  m3 <- sum((x-mu.x)^3)/n
  m4 <- sum((x-mu.x)^4)/n
  b1 <- m3/m2^(3/2)
  b2 <- m4/m2^2
  
  # objects to store results
  local.s.vec <- numeric(n)
  E.S.vec <- numeric(n)
  #	E.S.on.vec <- numeric(n)
  #	E.S.off.vec <- numeric(n)
  VAR.S.vec <- numeric(n)
  #	VAR.S.on.vec <- numeric(n)
  #	VAR.S.off.vec <- numeric(n)
  #	COV.S.2.vec <- numeric(n)
  z.x.ij.sum.vec <- numeric(n) 
  
  # For each observation
  for (i in 1:n){
    
    vi.vec <- listw$weights[[i]]
    vi.nhbr.vec <- listw$neighbours[[i]]
    vi.s <- sum(vi.vec)
    vi.s.2 <- vi.s^2
    vi.s.4 <- vi.s^4
    vi.2.s <- sum(vi.vec^2)
    vi.3.s <- sum(vi.vec^3)
    vi.2.ss <- vi.2.s^2
    vi.4.s <- sum(vi.vec^4)
    vii <- vii.vec[i]
    vii.2 <- vii^2
    vii.3 <- vii^3
    vii.4 <- vii^4
    
    z.x.i <- z.x[i]
    z.x.i.2 <- z.x.i^2
    z.x.i.4 <- z.x.i^4
    
    z.x.j <- z.x[vi.nhbr.vec]
    z.x.ij.sum <- sum(z.x.j*vi.vec)
    z.x.ij.sum.vec[i] <- z.x.ij.sum
    
    # calculating a local S value
    local.s <- K*(z.x.ij.sum^2)
    local.s.vec[i] <- local.s
    
    # calculating an expectation and variance
    if (method=="conditional") {
      diag.value <- K*(vii^2)*(z.x.i^2)
      E.S.off <- (K/((n-1)*(n-2)))*((vi.2.s-vii.2)-(vi.s-vii)^2)*(n-(2*z.x.i.2))
      if (vii==0)	{
        E.S.on <- K*(vi.2.s-vii.2)*(n-z.x.i.2)/(n-1)
        VAR.S.on <- (K^2)*(((vi.4.s*(n*b2-z.x.i.4))/(n-1))+(((vi.2.ss-vi.4.s)*((n-b2)*n-(2*(n-z.x.i.2))*z.x.i.2))/((n-1)*(n-2))))-(E.S.on^2)
        COV.1 <- ((K^2)/((n-1)*(n-2)))*(2*((vi.4.s-(vi.s*vi.3.s)))*((b2+(b1*z.x.i))*n-(2*z.x.i.4)))
        COV.2 <- ((K^2)/((n-1)*(n-2)*(n-3)))*(((vi.2.s*(vi.s.2-vi.2.s))+(2*(vi.4.s-(vi.s*vi.3.s))))*((((2*b2)-n+(2*b1*z.x.i))*n)+((3*(n-(2*z.x.i.2)))*z.x.i.2)))
      } else {
        E.S.on <- K*(vi.2.s-vii.2)*(n-3*z.x.i.2)/(n-1)
        VAR.S.on <- (K^2)*((((vi.4.s-vii.4)*(((b2+4*b1*z.x.i)*n)+((4*n-9*z.x.i.2)*z.x.i.2)))/(n-1))+((((vi.2.s-vii.2)^2-(vi.4.s-vii.4))*(((n-b2-4*b1*z.x.i)*n)-(2*(5*n-9*z.x.i.2)*z.x.i.2)))/((n-1)*(n-2))))-(E.S.on^2)
        COV.1 <- ((K^2)/((n-1)*(n-2)))*(2*((vi.4.s-vii.4)-(vi.s-vii)*(vi.3.s-vii.3))*((b2+3*b1*z.x.i)*n+(2*(n-3*z.x.i.2)*z.x.i.2)))
        COV.2 <- ((K^2)/((n-1)*(n-2)*(n-3)))*((((vi.2.s-vii.2)^2)-(((vi.s-vii)^2)*(vi.2.s-vii.2))-(2*(vi.4.s-vii.4))+(2*(vi.s-vii)*(vi.3.s-vii.3)))*(((n-(2*b2)-(6*b1*z.x.i))*n)-((9*(n-(2*z.x.i.2)))*z.x.i.2)))
      }
      
      E.S <- E.S.off + E.S.on + diag.value
      VAR.S.off.1 <- ((K^2)/((n-1)*(n-2)))*((2*((vi.2.s-vii.2)^2-(vi.4.s-vii.4)))*(((n-b2)*n)-(2*(n-z.x.i.2)*z.x.i.2)))
      VAR.S.off.2 <- ((4*(K^2))/((n-1)*(n-2)*(n-3)))*((((vi.s-vii)^2-(vi.2.s-vii.2))*(vi.2.s-vii.2))-(2*(vi.s-vii)*(vi.3.s-vii.3))+(2*(vi.4.s-vii.4)))*((((2*b2)-n+(2*b1*z.x.i))*n)+(3*(n-(2*z.x.i.2))*z.x.i.2))
      VAR.S.off.3 <- ((K^2)/((n-1)*(n-2)*(n-3)*(n-4)))*((((vi.s-vii)^4)+(3*((vi.2.s-vii.2)^2))-(6*((vi.s-vii)^2)*(vi.2.s-vii.2))-(6*(vi.4.s-vii.4))+(8*(vi.s-vii)*(vi.3.s-vii.3)))*(((3*n-(6*b2)-(8*b1*z.x.i))*n)-(12*(n-2*z.x.i.2)*z.x.i.2)))
      VAR.S.off <- VAR.S.off.1 + VAR.S.off.2 + VAR.S.off.3 - (E.S.off^2)
      COV.S <- (COV.1+COV.2)-(E.S.off*E.S.on)
      COV.S.2 <- 2*COV.S
      VAR.S <- VAR.S.off + VAR.S.on + COV.S.2
      
    }
    else if (method=="total") {
      
      E.S.off <- (-1)*K*(vi.s.2-vi.2.s)/(n-1)
      E.S.on <- K*vi.2.s
      E.S <- E.S.off + E.S.on
      VAR.S.off.1 <- (K^2)*(2*(vi.2.ss-vi.4.s)*((n-b2)/(n-1)))
      VAR.S.off.2 <- (K^2)*(-4)*((2*vi.4.s-(2*vi.s*vi.3.s)+((vi.s.2-vi.2.s)*vi.2.s)))*((n-(2*b2))/((n-1)*(n-2)))
      VAR.S.off.3 <- (K^2)*3*(vi.s.4-(6*vi.4.s)+(8*vi.s*vi.3.s)+(3*(vi.2.s-(2*vi.s.2))*vi.2.s))*((n-2*b2)/((n-1)*(n-2)*(n-3)))
      
      VAR.S.off <- VAR.S.off.1 + VAR.S.off.2 + VAR.S.off.3 - (E.S.off^2)
      VAR.S.on <- (K^2)*((vi.4.s*n)-(vi.2.ss))*(b2-1)/(n-1)
      COV.S <- (K^2)*((n*(vi.4.s-(vi.s*vi.3.s)))+((vi.s.2-vi.2.s)*(vi.2.s)))*2*(b2-1)/((n-1)*(n-2))
      COV.S.2 <- 2*COV.S
      VAR.S <- VAR.S.off + VAR.S.on + COV.S.2
      
    }
    else {
      
      V.i <- matrix(rep(0, n^2), ncol=n)
      V.i[i,] <- V[i,]
      
      #		numerator <- n*t(z.x)%*%(t(V.i)%*%V.i)%*%z.x
      #		leeS <- as.numeric(numerator/denominator)
      #		leeS.vec[i] <- leeS
      
      T.i <- (n^2)*crossprod(V.i)/sum.VtV
      K.i <- M%*%T.i
      
      tr <- function (a)
      {
        sum(diag(a))
      }
      
      mu.1 <- tr(K.i)/(n-1)
      mu.2 <- 2*(((n-1)*tr(K.i%*%K.i))-(tr(K.i)^2))/(((n-1)^2)*(n+1))
      mu.3 <- 8*(((n-1)^2*tr(K.i%*%K.i%*%K.i))-(3*(n-1)*tr(K.i)*tr(K.i%*%K.i))+(2*(tr(K.i)^3)))/((n-1)^3*(n+1)*(n+3))
      mu.4 <- 12*((((n-1)^3)*(4*tr(K.i%*%K.i%*%K.i%*%K.i)+(tr(K.i%*%K.i)^2)))-((2*((n-1)^2))*((8*tr(K.i)*tr(K.i%*%K.i%*%K.i))+(tr(K.i%*%K.i)*(tr(K.i)^2))))+((n-1)*((24*tr(K.i%*%K.i)*(tr(K.i)^2))+(tr(K.i)^4)))-(12*(tr(K.i)^4)))/(((n-1)^4)*(n+1)*(n+3)*(n+5))
      
      E.S <- mu.1
      VAR.S <- mu.2
      SKEW.S <- mu.3/((mu.2)^(3/2))
      KURT.S <- mu.4/((mu.2)^2)
      
    }
    
    # store values
    E.S.vec[i] <- E.S
    #	  E.S.on.vec[i] <- E.S.on
    #	  E.S.off.vec[i] <- E.S.off
    VAR.S.vec[i] <- VAR.S
    #	  VAR.S.on.vec[i] <- VAR.S.on
    #	  VAR.S.off.vec[i] <- VAR.S.off
    #	  COV.S.2.vec[i] <- COV.S.2
  }
  
  SD.S.vec <- sqrt(VAR.S.vec)
  Z.S.vec <- (local.s.vec-E.S.vec)/SD.S.vec
  
  # calculating p-values
  #P.S.vec <- numeric(n)
  if (alternative=="two.sided") {
    P.S.vec <- sapply(Z.S.vec, function(x) {2 * pnorm(abs(x), lower.tail = FALSE)})}
  else if (alternative=="greater") {
    P.S.vec <- sapply(Z.S.vec, function(x) {pnorm(x, lower.tail = FALSE)})}
  else {
    P.S.vec <- sapply(Z.S.vec, function(x) {pnorm(x, lower.tail = TRUE)})}
  
  # an analogy of Moran scatterplot
  class.dev <- character(n)
  for (q in 1:n){
    if (z.x.ij.sum.vec[q] < 0)
      class.dev[q] <- "Low"
    else class.dev[q] <- "High"
  }
  
  # Significance for different levels
  sig.df <- as.data.frame(sapply(sig.levels, function(x, p.l, c.d) {ifelse(p.l > x, "not sig.", c.d)}, p.l=P.S.vec, c.d=class.dev))
  colnames(sig.df) <- paste("sig", sig.levels, sep="")
  
  local.s.res <- data.frame(id=id, "local.S"=local.s.vec, sma.z.x=z.x.ij.sum.vec, expectation=E.S.vec, variance=VAR.S.vec,
                            pvalue=P.S.vec, quad=class.dev, sig.df)
  
  return(local.s.res)
  
}

L.global.mod <- function(x, y, listw){
  
  mf <- match.call(expand.dots = FALSE)
  
  # caluculating L statistic
  row.sum <- unlist(lapply(listw$weights,sum))
  nw <- sum(row.sum^2)
  n <- length(x)
  d.x <- x - mean(x)
  d.y <- y - mean(y)
  z.x <- as.matrix(d.x/(sqrt(sum(d.x^2)/n)))
  z.y <- as.matrix(d.y/(sqrt(sum(d.y^2)/n)))
  lx <- lag.listw(listw,d.x)
  ly <- lag.listw(listw,d.y)
  lee.l <- (n/nw)*sum(lx*ly)/(sqrt(sum((d.x)^2)) * sqrt(sum((d.y)^2)))
  return(lee.l)
}


L.local.mod <- function (x, y, listw){
  
  mf <- match.call(expand.dots = FALSE)
  
  n <- length(x)
  
  row.sum <- unlist(lapply(listw$weights,sum))
  sum.VtV <- sum(row.sum^2)
  
  # some basic calculations
  mu.x <- mean(x)
  mu.y <- mean(y)
  sd.x <- sqrt(sum((x - mu.x)^2)/n)
  sd.y <- sqrt(sum((y - mu.y)^2)/n)
  z.x <- (x-mu.x)/sd.x
  z.y <- (y-mu.y)/sd.y
  K <- n/sum.VtV
  
  # objects to store results
  local.lee.vec <- numeric(n)
  
  # For each observation
  for (i in 1:n){
    
    vi.vec <- listw$weights[[i]]
    vi.nhbr.vec <- listw$neighbours[[i]]
    z.x.j <- z.x[vi.nhbr.vec]
    z.y.j <- z.y[vi.nhbr.vec]
    z.x.ij.sum <- sum(z.x.j*vi.vec)
    z.y.ij.sum <- sum(z.y.j*vi.vec)
    
    # calculating a local L value
    local.lee.l <- K*(z.x.ij.sum*z.y.ij.sum)
    local.lee.vec[i] <- local.lee.l
  }
  local.lee.vec
}

L.global.test.mod <- function(x, y, listw, alternative="two.sided"){
  
  
  alternative <- match.arg(alternative, c("greater", "less", "two.sided"))
  #style <- match.arg(style, c("B","C","S","W","U"))
  mf <- match.call(expand.dots = FALSE)
  
  # caluculating L statistic
  
  W <- as(as(as_dgRMatrix_listw(listw), "RsparseMatrix"), "CsparseMatrix")
  nw <- sum(rowSums(W)^2)
  n <- length(x)
  d.x <- x - mean(x)
  d.y <- y - mean(y)
  z.x <- as.matrix(d.x/(sqrt(sum(d.x^2)/n)))
  z.y <- as.matrix(d.y/(sqrt(sum(d.y^2)/n)))
  lx <- lag.listw(listw,d.x)
  ly <- lag.listw(listw,d.y)
  lee.l <- (n/nw)*sum(lx*ly)/(sqrt(sum((d.x)^2)) * sqrt(sum((d.y)^2)))
  
  # Calculating a reference distribution
  W <- crossprod(W)
  W <- W/sum(W)
  W <- (W + t(W))/2
  
  p.on <- diag(W)
  p.rs <- rowSums(W)
  f2.all <- sum(p.rs^2)
  
  f2.on <- sum(p.on*p.rs)
  f0.on <- sum(p.on)
  f1.on <- sum(p.on^2)
  
  diag(W) <- 0
  f0.off <- sum(W)
  f1.off <- sum(W^2)
  f2.off <- sum(rowSums(W)^2)
  
  f0.all <- f0.off + f0.on
  f1.all <- f1.off + f1.on
  
  q.rs <- rep(0,n)
  g1.off <- 0
  for (i in 1:n) {
    q.i <- ((z.x%*%z.y[i]+z.y%*%z.x[i])/2)[-i]
    q.rs[i] <- sum(q.i)
    g1.off <- g1.off + sum(q.i^2)
  }
  
  g0.off <- sum(q.rs)
  g2.off <- sum(q.rs^2)
  
  q.on <- z.x * z.y
  g0.on <- sum(q.on)
  g1.on <- sum(q.on^2)
  g2.on <- sum(q.on*q.rs)
  
  g0.all <- g0.off + g0.on
  g1.all <- g1.off + g1.on
  g2.all <- sum((q.rs + q.on)^2)
  
  # Expected values
  E.L.off <- (f0.off*g0.off)/(n*(n-1))
  E.L.on <- (f0.on*g0.on)/n
  E.L <- E.L.off + E.L.on
  
  # Variance
  VAR.L.off <- ((2*f1.off*g1.off)/(n*(n-1)))+((4*(f2.off-f1.off)*(g2.off-g1.off))/(n*(n-1)*(n-2)))+ ((((f0.off^2)+(2*f1.off)-(4*f2.off))*((g0.off^2)+(2*g1.off)-(4*g2.off)))/(n*(n-1)*(n-2)*(n-3)))-(E.L.off^2)
  VAR.L.on <- (f1.on*g1.on/n)+(((f0.on^2-f1.on)*(g0.on^2-g1.on))/(n*(n-1)))-(E.L.on^2)
  COV.1 <- ((f2.all-f1.on-f2.off)*(g2.all-g1.on-g2.off))/(2*n*(n-1))
  COV.2 <- (((f0.on*f0.off)-(f2.all-f1.on-f2.off))*((g0.on*g0.off)-(g2.all-g1.on-g2.off)))/(n*(n-1)*(n-2))
  COV.L <- (COV.1+COV.2)-(E.L.off*E.L.on)
  COV.L.2 <- 2*COV.L
  
  VAR.L <- VAR.L.off + VAR.L.on + COV.L.2
  
  SD.L <- sqrt(VAR.L)
  Z.L <- (lee.l - E.L)/SD.L
  
  # Calculation p-values
  if (alternative=="two.sided")
    P.L <- 2 * pnorm(abs(Z.L), lower.tail = FALSE)
  else if (alternative=="greater")
    P.L <- pnorm(Z.L, lower.tail = FALSE)
  else P.L <- pnorm(Z.L)
  
  statistic <- Z.L
  attr(statistic, "names") <- "Lee's L statistic standard deviate"
  p.value <- P.L
  estimate <- c(lee.l, E.L, VAR.L)
  attr(estimate, "names") <- c("Observed Lee's L", "Expectation","Variance")
  method <- "Global Lee's L test under randomization"
  
  #if (diag.zero==FALSE) 
  style <- paste("W*",sep="")
  data.name <- paste(deparse(mf[[2]]), " & ",  deparse(mf[[3]]), "\nweights: ",  deparse(mf[[4]]), ", style: ", style, "\n", sep = "")
  res <- list(statistic = statistic, p.value = p.value, estimate = estimate,
              method = method, alternative = alternative, data.name = data.name)
  class(res) <- "htest"
  res
}


S.local.mod <- function (x, listw){
  
  mf <- match.call(expand.dots = FALSE)
  
  n <- length(x)
  
  row.sum <- unlist(lapply(listw$weights,sum))
  sum.VtV <- sum(row.sum^2)
  
  # some basic calculations
  mu.x <- mean(x)
  sd.x <- sqrt(sum((x - mu.x)^2)/n)
  z.x <- (x-mu.x)/sd.x
  K <- n/sum.VtV
  
  # objects to store results
  local.s.vec <- numeric(n)
  
  # For each observation
  for (i in 1:n){
    
    vi.vec <- listw$weights[[i]]
    vi.nhbr.vec <- listw$neighbours[[i]]
    z.x.j <- z.x[vi.nhbr.vec]
    z.x.ij.sum <- sum(z.x.j*vi.vec)
    # calculating a local S value
    local.s <- K*(z.x.ij.sum^2)
    local.s.vec[i] <- local.s
  }
  local.s.vec
}

S.global.test.mod <- function(x, listw, method="randomization", alternative="two.sided"){
  # purpose: Calculate Global Lee's L and test statistic under randomization
  # Arguments:
  #   x: a vector of one variable
  #   nb: nb object
  #   style: style of spatial weight matrix (refer to nb2listw)
  #   method: "normality" or "randomization" assumption for null hypothesis 
  #   alternatie: type of test
  #   diag.zero: if FALSE, each region has itself as its neighbour
  #   Fuction called:
  #   nb.self
  
  alternative <- match.arg(alternative, c("greater", "less", "two.sided"))
  
  method <- match.arg(method, c("normality", "randomization"))
  mf <- match.call(expand.dots = FALSE)
  
  
  #  if (style != "W") {
  #  lee.s <- (n/nw)*lee.s
  #  }
  
  # calculating S statistic
  
  V <- as(as(as_dgRMatrix_listw(listw), "RsparseMatrix"), "CsparseMatrix")
  nw <- sum(rowSums(V)^2)
  n <- length(x)
  d.x <- x - mean(x)
  z.x <- as.matrix(d.x/(sqrt(sum(d.x^2)/n)))
  lx <- lag.listw(listw,d.x)
  lee.s <- (n/nw)*sum(lx^2)/sum((d.x)^2)
  
  # Calculating a reference distribution
  
  if (method=="randomization") {
    
    V <- crossprod(V)
    V <- V/sum(V)
    V <- (V + t(V))/2
    p.on <- diag(V)
    p.rs <- rowSums(V)
    f2.all <- sum(p.rs^2)
    
    f2.on <- sum(p.on*p.rs)
    f0.on <- sum(p.on)
    f1.on <- sum(p.on^2)
    
    diag(V) <- 0
    f0.off <- sum(V)
    f1.off <- sum(V^2)
    f2.off <- sum(rowSums(V)^2)
    
    f0.all <- f0.off + f0.on
    f1.all <- f1.off + f1.on
    
    q.rs <- rep(0,n)
    g1.off <- 0
    for (i in 1:n) {
      q.i <- ((z.x%*%z.x[i]+z.x%*%z.x[i])/2)[-i]
      q.rs[i] <- sum(q.i)
      g1.off <- g1.off + sum(q.i^2)
    }
    
    g0.off <- sum(q.rs)
    g2.off <- sum(q.rs^2)
    
    q.on <- z.x^2
    g0.on <- sum(q.on)
    g1.on <- sum(q.on^2)
    g2.on <- sum(q.on*q.rs)
    
    g0.all <- g0.off + g0.on
    g1.all <- g1.off + g1.on
    g2.all <- sum((q.rs + q.on)^2)
    
    # Expected values
    E.S.off <- (f0.off*g0.off)/(n*(n-1))
    E.S.on <- (f0.on*g0.on)/n
    E.S <- E.S.off + E.S.on
    
    # Variance
    VAR.S.off <- ((2*f1.off*g1.off)/(n*(n-1)))+((4*(f2.off-f1.off)*(g2.off-g1.off))/(n*(n-1)*(n-2)))+ ((((f0.off^2)+(2*f1.off)-(4*f2.off))*((g0.off^2)+(2*g1.off)-(4*g2.off)))/(n*(n-1)*(n-2)*(n-3)))-(E.S.off^2)
    VAR.S.on <- (f1.on*g1.on/n)+(((f0.on^2-f1.on)*(g0.on^2-g1.on))/(n*(n-1)))-(E.S.on^2)
    COV.1 <- ((f2.all-f1.on-f2.off)*(g2.all-g1.on-g2.off))/(2*n*(n-1))
    COV.2 <- (((f0.on*f0.off)-(f2.all-f1.on-f2.off))*((g0.on*g0.off)-(g2.all-g1.on-g2.off)))/(n*(n-1)*(n-2))
    COV.S <- (COV.1+COV.2)-(E.S.off*E.S.on)
    COV.S.2 <- 2*COV.S
    
    VAR.S <- VAR.S.off + VAR.S.on + COV.S.2
    
    SD.S <- sqrt(VAR.S)
    Z.S <- (lee.s - E.S)/SD.S
  }
  
  else {
    
    V <- crossprod(V)
    one.vec <- rep(1,n)
    #	one.matrix <- as.matrix(rep(1,n))
    #	z.x.matrix <- as.matrix(z.x)
    numerator <- t(z.x)%*%V%*%z.x
    denominator <- t(one.vec)%*%V%*%one.vec
    lee.s <- as.numeric(numerator/denominator)
    
    M <- diag(n)-(one.vec%*%solve(crossprod(one.vec))%*%t(one.vec))
    V.s <- (n*(V))/sum(V)
    K <- M%*%V.s
    
    tr <- function (a)
    {
      sum(diag(a))
    }
    
    mu.1 <- tr(K)/(n-1)
    mu.2 <- 2*(((n-1)*tr(K%*%K))-(tr(K)^2))/(((n-1)^2)*(n+1))
    mu.3 <- 8*(((n-1)^2*tr(K%*%K%*%K))-(3*(n-1)*tr(K)*tr(K%*%K))+(2*(tr(K)^3)))/((n-1)^3*(n+1)*(n+3))
    mu.4 <- 12*((((n-1)^3)*(4*tr(K%*%K%*%K%*%K)+(tr(K%*%K)^2)))-((2*((n-1)^2))*((8*tr(K)*tr(K%*%K%*%K))+(tr(K%*%K)*(tr(K)^2))))+((n-1)*((24*tr(K%*%K)*(tr(K)^2))+(tr(K)^4)))-(12*(tr(K)^4)))/(((n-1)^4)*(n+1)*(n+3)*(n+5))
    
    E.S <- mu.1
    VAR.S <- mu.2
    SD.S <- sqrt(mu.2)
    SKEW.S <- mu.3/((mu.2)^(3/2))
    KURT.S <- mu.4/((mu.2)^2)
    Z.S <- (lee.s - E.S)/SD.S
  }
  
  
  # Calculation p-values
  if (alternative=="two.sided")
    P.S <- 2 * pnorm(abs(Z.S), lower.tail = FALSE)
  else if (alternative=="greater")
    P.S <- pnorm(Z.S, lower.tail = FALSE)
  else P.S <- pnorm(Z.S)
  
  statistic <- Z.S
  attr(statistic, "names") <- "Lee's S statistic standard deviate"
  p.value <- P.S
  estimate <- c(lee.s, E.S, VAR.S)
  attr(estimate, "names") <- c("Observed Lee's S", "Expectation","Variance")
  
  if (method=="randomization") {
    method <- "Global Lee's S test under randomization"
  }
  else {
    method <- "Global Lee's S test under normality"
  }
  
  #if (diag.zero==FALSE) style <- paste(style, "*",sep="")
  data.name <- paste(deparse(mf[[2]]), " & ",  deparse(mf[[3]]), "\nweights: ",  deparse(mf[[4]]), ", style: W*", "\n", sep = "")
  res <- list(statistic = statistic, p.value = p.value, estimate = estimate,
              method = method, alternative = alternative, data.name = data.name)
  class(res) <- "htest"
  res
}


S.global.mod <- function(x, listw){
  
  mf <- match.call(expand.dots = FALSE)
  
  # calculating S statistic
  row.sum <- unlist(lapply(listw$weights,sum))
  nw <- sum(row.sum^2)
  n <- length(x)
  d.x <- x - mean(x)
  z.x <- as.matrix(d.x/(sqrt(sum(d.x^2)/n)))
  lx <- lag.listw(listw,d.x)
  lee.s <- (n/nw)*sum(lx^2)/sum((d.x)^2)
  return(lee.s)
}

##Addictive nblags.star for clustogram 
add.nblags.star <- function(nblags){
  nlags <- length(nblags)+1
  n <- length(nblags[[2]])
  
  nblags.star <-vector("list", nlags)
  for(i in 1:nlags){
    if(i==1){
      nblags.star[[i]] <- nblags[[1]]
      for(j in 1:n){
        nblags.star[[i]][[j]] <- as.integer(j)
      }
    }
    else if(i==2){
      nblags.star[[i]] <- nblags[[1]]
      for(j in 1:n){
        nblags.star[[i]][[j]] <- as.integer(sort(c(nblags[[i-1]][[j]], j)))
      }
    }
    else{
      nblags.star[[i]] <- nblags[[1]]
      for(j in 1:n){
        nblags.star[[i]][[j]] <- as.integer(sort(c(nblags[[i-2]][[j]],nblags[[i-1]][[j]])))
      }
    }
  }
  nblags.star
}

##Addictive nblags.star for clustogram 
add.nblags.new <- function(nblags){
  nlags <- length(nblags)+1
  n <- length(nblags[[2]])
  
  nblags.star <-vector("list", nlags)
  for(i in 1:nlags){
    if(i==1){
      nblags.star[[i]] <- nblags[[1]]
      for(j in 1:n){
        nblags.star[[i]][[j]] <- as.integer(j)
      }
    }
    else if(i==2){
      nblags.star[[i]] <- nblags[[1]]
      for(j in 1:n){
        nblags.star[[i]][[j]] <- as.integer(sort(c(nblags[[i-1]][[j]])))
      }
    }
    else{
      nblags.star[[i]] <- nblags[[1]]
      for(j in 1:n){
        nblags.star[[i]][[j]] <- as.integer(sort(c(nblags[[i-2]][[j]],nblags[[i-1]][[j]])))
      }
    }
  }
  nblags.star
}

##Addictive nblags. for clustogram 
add.nblags <- function(nblags){
  nlags <- length(nblags)
  n <- length(nblags[[1]])
  
  for(i in 1:nlags){
    if(i==1){
      for(j in 1:n){
        nblags[[i]][[j]] <- as.integer(sort(c(nblags[[i]][[j]], j)))
      }
    }
    else{
      for(j in 1:n){
        nblags[[i]][[j]] <- as.integer(sort(c(nblags[[i-1]][[j]],nblags[[i]][[j]])))
      }
    }
  }
  nblags
}
