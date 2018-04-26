# Program Purpose: 
#   Calculation of global / local Lee's L values and corresponding tests.
# Reference:
#   Sang-Il Lee (2001) "Developing a bivariate spatial association measure: 
#       an integration of Pearson's r and Moran's I", Journal of Geograhical Systems, 3:369-385
#   Sang-Il Lee (2004) "A generalized significance testing method for global measures  
#       of spatial association: an extension of the Mantel test", Environment and Planning A 36:1687-1703
#   Sang-Il Lee (2009) "A generalized randomization approach to local 
#       measures of spatial association", Geographical Analysis 41:221-248
#
# Date: 2016. 11. 22
# Author: Sang-Il Lee and Yongwan Chun
  
nb.self <- function (nb) {
  # purpose: Making non-zero diagonal elements in spatial weight matrix
  # Arguments:  
  #   nb: nb object

	n <- length(nb)
  for (i in 1:n) nb[[i]] <- sort(c(i,nb[[i]]))
  return(nb)
}

L.global.mod <- function(x, y, listw){
  # purpose: Calculate Global Lee's L 
  # Arguments:
  #   x: a vector of one variable
  #   y: a vector of another variable
  #   nb: nb object
  #   style: style of spatial weight matrix (refer to nb2listw)
  #   diag.zero: if FALSE, each region has itself as its neighbour
  # Fuction called:
  #   nb.self
  
  #style <- match.arg(style, c("B","C","S","W","U"))
  mf <- match.call(expand.dots = FALSE)
  
  # making non-zero diagonal elements in a spatial weight matrix
  #if (diag.zero == FALSE) nb <- nb.self(nb)
  #listw <- nb2listw(nb,style=style)
  
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


L.global <- function(x, y, nb, style="W", diag.zero=FALSE){
  # purpose: Calculate Global Lee's L 
  # Arguments:
  #   x: a vector of one variable
  #   y: a vector of another variable
  #   nb: nb object
  #   style: style of spatial weight matrix (refer to nb2listw)
  #   diag.zero: if FALSE, each region has itself as its neighbour
  # Fuction called:
  #   nb.self

  style <- match.arg(style, c("B","C","S","W","U"))
  mf <- match.call(expand.dots = FALSE)

  # making non-zero diagonal elements in a spatial weight matrix
  if (diag.zero == FALSE) nb <- nb.self(nb)
  listw <- nb2listw(nb,style=style)

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
  # purpose: Calculate local Lee's L 
  # Arguments:
  #   x: a vector of one variable
  #   y: a vector of another variable
  #   id: a sequence of 1:n (n is the number of observations)
  #   nb: nb object
  #   style: style of spatial weight matrix (refer to nb2listw)
  #   diag.zero: if FALSE, each region has itself as its neighbour
  # 	Fuction called:
  #   nb.self
  
  #style <- match.arg(style, c("B","C","S","W","U"))
  mf <- match.call(expand.dots = FALSE)
  
  n <- length(x)
  
  # making non-zero diagonal elements in a spatial weight matrix
  #if (diag.zero == FALSE) nb <- nb.self(nb)
  #listw <- nb2listw(nb,style=style)
  row.sum <- unlist(lapply(listw$weights,sum))
  sum.VtV <- sum(row.sum^2)
  
  # dealing with diagonal elements
  #vii.vec <- rep(0,n)
  #if (diag.zero == FALSE) {
   # iis <- unlist(lapply(1:n, function (x,nbs) match(x,nbs[[x]]), nbs=listw$neighbours))
    #for (i in 1:n) vii.vec[i] <- listw$weights[[i]][iis[i]]
  #}
  
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


L.local <- function (x, y, id, nb, style="W", diag.zero=FALSE){
  # purpose: Calculate local Lee's L 
  # Arguments:
  #   x: a vector of one variable
  #   y: a vector of another variable
  #   id: a sequence of 1:n (n is the number of observations)
  #   nb: nb object
  #   style: style of spatial weight matrix (refer to nb2listw)
  #   diag.zero: if FALSE, each region has itself as its neighbour
  # 	Fuction called:
  #   nb.self

  style <- match.arg(style, c("B","C","S","W","U"))
  mf <- match.call(expand.dots = FALSE)

  n <- length(x)

  # making non-zero diagonal elements in a spatial weight matrix
  if (diag.zero == FALSE) nb <- nb.self(nb)
  listw <- nb2listw(nb,style=style)
  row.sum <- unlist(lapply(listw$weights,sum))
  sum.VtV <- sum(row.sum^2)

  # dealing with diagonal elements
  vii.vec <- rep(0,n)
  if (diag.zero == FALSE) {
    iis <- unlist(lapply(1:n, function (x,nbs) match(x,nbs[[x]]), nbs=listw$neighbours))
    for (i in 1:n) vii.vec[i] <- listw$weights[[i]][iis[i]]
  }

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
  # purpose: Calculate Global Lee's L and test statistic under randomization
  # Arguments:
  #   x: a vector of one variable
  #   y: a vector of another variable
  #   nb: nb object
  #   style: style of spatial weight matrix (refer to nb2listw)
  #   alternatie: type of test
  #   diag.zero: if FALSE, each region has itself as its neighbour
  # Fuction called:
  #   nb.self
  
  alternative <- match.arg(alternative, c("greater", "less", "two.sided"))
  #style <- match.arg(style, c("B","C","S","W","U"))
  mf <- match.call(expand.dots = FALSE)
  
  # making non-zero diagonal elements in a spatial weight matrix
  #if (diag.zero == FALSE) nb <- nb.self(nb)
  #listw <- nb2listw(nb,style=style)
  #	if (style != "W") {
  #   lee.l <- (n/nw)*lee.l
  #	}
  
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




L.global.test <- function(x, y, nb, style="W", alternative="two.sided", diag.zero=FALSE){
  # purpose: Calculate Global Lee's L and test statistic under randomization
  # Arguments:
  #   x: a vector of one variable
  #   y: a vector of another variable
  #   nb: nb object
  #   style: style of spatial weight matrix (refer to nb2listw)
  #   alternatie: type of test
  #   diag.zero: if FALSE, each region has itself as its neighbour
  # Fuction called:
  #   nb.self

  alternative <- match.arg(alternative, c("greater", "less", "two.sided"))
  style <- match.arg(style, c("B","C","S","W","U"))
  mf <- match.call(expand.dots = FALSE)

  # making non-zero diagonal elements in a spatial weight matrix
  if (diag.zero == FALSE) nb <- nb.self(nb)
  listw <- nb2listw(nb,style=style)
  #	if (style != "W") {
  #   lee.l <- (n/nw)*lee.l
  #	}

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

  if (diag.zero==FALSE) style <- paste(style, "*",sep="")
  data.name <- paste(deparse(mf[[2]]), " & ",  deparse(mf[[3]]), "\nweights: ",  deparse(mf[[4]]), ", style: ", style, "\n", sep = "")
  res <- list(statistic = statistic, p.value = p.value, estimate = estimate,
        method = method, alternative = alternative, data.name = data.name)
  class(res) <- "htest"
  res
}

L.local.test <- function (x, y, id, nb, style="W", sig.levels=c(0.05), method="Conditional", alternative="two.sided", diag.zero=FALSE){
  # purpose: Calculate local Lee's L and test statistic under conditional or total randomization
  # Arguments:
  #   x: a vector of one variable
  #   y: a vector of another variable
  #   id: a sequence of 1:n (n is the number of observations)
  #   nb: nb object
  #   sig: a vector of significance levels	
  #   style: style of spatial weight matrix (refer to nb2listw)
  #   method: "conditional" or "total" randomization assumption for null hypothesis
  #   alternatie: type of test
  #   diag.zero: if FALSE, each region has itself as its neighbour
  # Fuction called:
  #   nb.self

  alternative <- match.arg(alternative, c("greater", "less", "two.sided"))
  style <- match.arg(style, c("B","C","S","W","U"))
  method <- match.arg(method, c("Conditional", "Total"))
  mf <- match.call(expand.dots = FALSE)

  n <- length(x)

  # making non-zero diagonal elements in a spatial weight matrix
  if (diag.zero == FALSE) nb <- nb.self(nb)
  listw <- nb2listw(nb,style=style)

  row.sum <- unlist(lapply(listw$weights,sum))
	sum.VtV <- sum(row.sum^2)

  # dealing with diagonal elements
  vii.vec <- rep(0,n)
  if (diag.zero == FALSE) {
    iis <- unlist(lapply(1:n, function (x,nbs) match(x,nbs[[x]]), nbs=listw$neighbours))
    for (i in 1:n) vii.vec[i] <- listw$weights[[i]][iis[i]]
  }

  mu.x <- mean(x)
	mu.y <- mean(y)
	sd.x <- sqrt(sum((x - mu.x)^2)/n)
	sd.y <- sqrt(sum((y - mu.y)^2)/n)
	z.x <- (x-mu.x)/sd.x
	z.y <- (y-mu.y)/sd.y
	z.x.2 <- z.x^2
	z.y.2 <- z.y^2
	z.x.2.z.y.2 <- z.x.2+z.y.2
	z.xy.2 <- z.x.2*z.y.2

	K <- n/sum.VtV

	m1.xy <- sum((x - mu.x)*(y - mu.y))/n
	m2.xy <- sum(((x - mu.x)^2) * ((y - mu.y)^2))/n
	b1.xy <- m2.xy/m1.xy^2

	r.xy <- cor(x, y)
	r.xy.2 <- r.xy^2

	if (method=="conditional") {
  	m2.x <- sum((x - mu.x)^2)/n
	  m2.y <- sum((y - mu.y)^2)/n
  	m12.xy <- sum((x - mu.x) * ((y - mu.y)^2))/n
	  m21.xy <- sum(((x - mu.x)^2) * (y - mu.y))/n
    b12.xy <- m12.xy/(sqrt(m2.y)*m1.xy)
  	b21.xy <- m21.xy/(sqrt(m2.x)*m1.xy)
  }

  # objects to store results
	local.lee.vec <- numeric(n)
	E.L.vec <- numeric(n)
	E.L.on.vec <- numeric(n)
	E.L.off.vec <- numeric(n)
	VAR.L.vec <- numeric(n)
	VAR.L.on.vec <- numeric(n)
	VAR.L.off.vec <- numeric(n)
	COV.L.2.vec <- numeric(n)
  z.x.ij.sum.vec <- numeric(n)
	z.y.ij.sum.vec <- numeric(n)
  

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
    z.y.i <- z.y[i]
	 	z.x.i.2 <- z.x.i^2
  	z.x.i.4 <- z.x.i^4
		z.y.i.2 <- z.y.i^2
		z.y.i.4 <- z.y.i^4

		z.x.j <- z.x[vi.nhbr.vec]
		z.y.j <- z.y[vi.nhbr.vec]
		z.x.ij.sum <- sum(z.x.j*vi.vec)
		z.y.ij.sum <- sum(z.y.j*vi.vec)
		z.x.ij.sum.vec[i] <- z.x.ij.sum
		z.y.ij.sum.vec[i] <- z.y.ij.sum
	 	z.y.ij.sum <- sum(z.y.j*vi.vec)


  # calculating a local L value
		local.lee.l <- K*(z.x.ij.sum*z.y.ij.sum)
		local.lee.vec[i] <- local.lee.l

  # calculating an expectation and variance
		if (method=="conditional") {
  			diag.value <- K*(vii^2)*(z.x.i*z.y.i)
			  E.L.off <- (K/((n-1)*(n-2)))*((vi.2.s-vii.2)-(vi.s-vii)^2)*(n*r.xy-(2*z.x.i*z.y.i))
  			if (vii==0)	{
				   E.L.on <- K*(vi.2.s-vii.2)*(n*r.xy-(z.x.i*z.y.i))/(n-1)
				   VAR.L.on <- (K^2)*(((vi.4.s*(n*b1.xy*r.xy.2-(z.x.i.2*z.y.i.2)))/(n-1))+(((vi.2.ss-vi.4.s)*(((n*r.xy)-(b1.xy*r.xy)-(2*z.x.i*z.y.i))*n*r.xy+(2*z.x.i.2*z.y.i.2)))/((n-1)*(n-2))))-(E.L.on^2)
				   COV.1 <- ((K^2)/((n-1)*(n-2)))*(((vi.4.s-(vi.s*vi.3.s))))*((-4*z.x.i.2*z.y.i.2)+(((2*b1.xy*r.xy)+(z.x.i*b12.xy)+(z.y.i*b21.xy))*n*r.xy))
				   COV.2 <- ((K^2)/((n-1)*(n-2)*(n-3)))*((vi.2.s*(vi.s.2-vi.2.s))+(2*(vi.4.s-(vi.s*vi.3.s))))*(-6*z.x.i.2*z.y.i.2+(((-1*n*r.xy)+(3*z.x.i*z.y.i)+(2*b1.xy*r.xy)+(z.x.i*b12.xy)+(z.y.i*b21.xy))*n*r.xy))
			  } else {
				   E.L.on <- K*(vi.2.s-vii.2)*(n*r.xy-(3*z.x.i*z.y.i))/(n-1)
				   VAR.L.on <- (K^2)*((((vi.4.s-vii.4)*(((z.x.i.2+z.y.i.2)*n)-(9*z.x.i.2*z.y.i.2)+(((2*z.x.i*z.y.i)+(b1.xy*r.xy)+(2*z.x.i*b12.xy)+(2*z.y.i*b21.xy))*n*r.xy))/(n-1))+((((vi.2.s-vii.2)^2-(vi.4.s-vii.4))*(18*z.x.i.2*z.y.i.2-(n*(z.x.i.2+z.y.i.2))-(n*r.xy*((8*z.x.i*z.y.i)+(b1.xy*r.xy)+(2*z.x.i*b12.xy)+(2*z.y.i*b21.xy)-(n*r.xy)))))/((n-1)*(n-2)))))-(E.L.on^2)
				   COV.1 <- ((K^2)/((n-1)*(n-2)))*(((vi.4.s-vii.4)-(vi.s-vii)*(vi.3.s-vii.3)))*((-12*z.x.i.2*z.y.i.2)+((z.x.i.2+z.y.i.2)*n)+(((2*z.x.i*z.y.i)+(2*b1.xy*r.xy)+(3*z.x.i*b12.xy)+(3*z.y.i*b21.xy))*n*r.xy))
				   COV.2 <- ((K^2)/((n-1)*(n-2)*(n-3)))*(((vi.2.s-vii.2)^2)-(((vi.s-vii)^2)*(vi.2.s-vii.2))-(2*(vi.4.s-vii.4))+(2*(vi.s-vii)*(vi.3.s-vii.3)))*((18*z.x.i.2*z.y.i.2)-((z.x.i.2+z.y.i.2)*n)-(((-1*n*r.xy)+(7*z.x.i*z.y.i)+(2*b1.xy*r.xy)+(3*z.x.i*b12.xy)+(3*z.y.i*b21.xy))*n*r.xy))
			 }

			 E.L <- E.L.off + E.L.on + diag.value
			 VAR.L.off.1 <- ((K^2)/((n-1)*(n-2)))*((vi.2.s-vii.2)^2-(vi.4.s-vii.4))*(n^2-((z.x.i.2+z.y.i.2)*n)+(4*z.x.i.2*z.y.i.2)+(((n*r.xy)-(2*b1.xy*r.xy)-(2*z.x.i*z.y.i))*n*r.xy))
			 VAR.L.off.2 <- ((K^2)/((n-1)*(n-2)*(n-3)))*((((vi.s-vii)^2-(vi.2.s-vii.2))*(vi.2.s-vii.2))-(2*(vi.s-vii)*(vi.3.s-vii.3))+(2*(vi.4.s-vii.4)))*((((3*(z.x.i.2+z.y.i.2)*n)-(2*n^2)-(24*z.x.i.2*z.y.i.2)+(2*((4*b1.xy*r.xy)+(3*z.x.i*z.y.i)-(n*r.xy)+(2*z.x.i*b12.xy)+(2*z.y.i*b21.xy))*n*r.xy))))
	  	 VAR.L.off.3 <- ((K^2)/((n-1)*(n-2)*(n-3)*(n-4)))*((((vi.s-vii)^4)+(3*((vi.2.s-vii.2)^2))-(6*((vi.s-vii)^2)*(vi.2.s-vii.2))-(6*(vi.4.s-vii.4))+(8*(vi.s-vii)*(vi.3.s-vii.3)))*((((n^2-2*(z.x.i.2+z.y.i.2)*n)+(24*z.x.i.2*z.y.i.2)+(2*((-3*b1.xy*r.xy)-(4*z.x.i*z.y.i)+(n*r.xy)-(2*z.x.i*b12.xy)-(2*z.y.i*b21.xy))*n*r.xy)))))
			 VAR.L.off <- VAR.L.off.1 + VAR.L.off.2 + VAR.L.off.3 - (E.L.off^2)

			 COV.L <- (COV.1+COV.2)-(E.L.off*E.L.on)
		}
		else {
		   E.L.off <- (-1)*K*r.xy*(vi.s.2-vi.2.s)/(n-1)
			 E.L.on <- K*r.xy*vi.2.s
			 E.L <- E.L.off + E.L.on
  		 VAR.L.off.1 <- (K^2)*((vi.2.ss-vi.4.s)*((n-((2*b1.xy-n)*r.xy.2))/(n-1)))
	 	   VAR.L.off.2 <- (K^2)*(-2)*((2*vi.4.s-(2*vi.s*vi.3.s)+((vi.s.2-vi.2.s)*vi.2.s)))*((n-((4*b1.xy-n)*r.xy.2))/((n-1)*(n-2)))
		   VAR.L.off.3 <- (K^2)*(vi.s.4-(6*vi.4.s)+(8*vi.s*vi.3.s)+(3*(vi.2.s-(2*vi.s.2))*vi.2.s))*(((n-(2*r.xy.2)*((3*b1.xy)-n)))/((n-1)*(n-2)*(n-3)))
		   VAR.L.off <- VAR.L.off.1 + VAR.L.off.2 + VAR.L.off.3 - (E.L.off^2)
		   VAR.L.on <- (K^2)*((vi.4.s*n)-(vi.2.ss))*r.xy.2*(b1.xy-1)/(n-1)
		   COV.L <- (K^2)*((n*(vi.4.s-(vi.s*vi.3.s)))+((vi.s.2-vi.2.s)*(vi.2.s)))*2*r.xy.2*(b1.xy-1)/((n-1)*(n-2))
    }
    COV.L.2 <- 2*COV.L
    VAR.L <- VAR.L.off + VAR.L.on + COV.L.2

    # store values
	  E.L.vec[i] <- E.L
	  E.L.on.vec[i] <- E.L.on
	  E.L.off.vec[i] <- E.L.off
	  VAR.L.vec[i] <- VAR.L
	  VAR.L.on.vec[i] <- VAR.L.on
	  VAR.L.off.vec[i] <- VAR.L.off
	  COV.L.2.vec[i] <- COV.L.2
	}

	SD.L.vec <- sqrt(VAR.L.vec)
	Z.L.vec <- (local.lee.vec-E.L.vec)/SD.L.vec

  # calculating p-values
	#P.L.vec <- numeric(n)
	if (alternative=="two.sided") {
	    P.L.vec <- sapply(Z.L.vec, function(x) {2 * pnorm(abs(x), lower.tail = FALSE)})}
	else if (alternative=="greater") {
   		P.L.vec <- sapply(Z.L.vec, function(x) {pnorm(x, lower.tail = FALSE)})}
	else {
      P.L.vec <- sapply(Z.L.vec, function(x) {pnorm(x, lower.tail = TRUE)})}

  # an analogy of Moran scatterplot
	class.dev <- character(n)
# 	for (q in 1:n){
# 		if (z.x.ij.sum.vec[q] >= 0 & z.y.ij.sum.vec[q] >= 0) {
#       	class.dev[q] <- "High-High"}
# 		else if (z.x.ij.sum.vec[q] < 0 & z.y.ij.sum.vec[q] >= 0) {
# 	  		class.dev[q] <- "Low-High"}
# 		else if (z.x.ij.sum.vec[q] < 0 & z.y.ij.sum.vec[q] < 0) {
# 			  class.dev[q] <- "Low-Low"}
#   	else if (z.x.ij.sum.vec[q] >= 0 & z.y.ij.sum.vec[q] < 0) {
# 	  		class.dev[q] <- "High-Low"}
# 	}
	for (q in 1:n){
	  if (z.x.ij.sum.vec[q] >= 0 & z.y.ij.sum.vec[q] >= 0) {
	    class.dev[q] <- "HH"}
	  else if (z.x.ij.sum.vec[q] < 0 & z.y.ij.sum.vec[q] >= 0) {
	    class.dev[q] <- "LH"}
	  else if (z.x.ij.sum.vec[q] < 0 & z.y.ij.sum.vec[q] < 0) {
	    class.dev[q] <- "LL"}
	  else if (z.x.ij.sum.vec[q] >= 0 & z.y.ij.sum.vec[q] < 0) {
	    class.dev[q] <- "HL"}
	}
  # Significance for different levels
	sig.df <- as.data.frame(sapply(sig.levels, function(x, p.l, c.d) {ifelse(p.l > x, "", c.d)}, p.l=P.L.vec, c.d=class.dev))
	#	sig.df <- as.data.frame(sapply(sig.levels, function(x, p.l, c.d) {ifelse(p.l > x, "not sig.", c.d)}, p.l=P.L.vec, c.d=class.dev))
	#colnames(sig.df) <- paste("sig", sig.levels, sep="")
	colnames(sig.df) <- "sig"
	local.lee <- data.frame(id=id, "local.L"=local.lee.vec, sma.z.x=z.x.ij.sum.vec, sma.z.y=z.y.ij.sum.vec, expectation=E.L.vec, variance=VAR.L.vec,
                 pvalue=P.L.vec, quad=class.dev, sig.df)

	return(local.lee)

}
