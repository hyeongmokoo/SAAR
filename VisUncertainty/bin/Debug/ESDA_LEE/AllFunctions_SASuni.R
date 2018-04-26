# Program Purpose: 
#   Conducting univariate SAS analyses.
# Reference:
#   Sang-Il Lee (2001) "Developing a bivariate spatial association measure: 
#       an integration of Pearson's r and Moran's I", Journal of Geograhical Systems, 3:369-385
#   Sang-Il Lee (2004) "A generalized significance testing method for global measures  
#       of spatial association: an extension of the Mantel test", Environment and Planning A 36:1687-1703
#   Sang-Il Lee (2009) "A generalized randomization approach to local 
#       measures of spatial association", Geographical Analysis 41:221-248
#   Sang-Il Lee (2017) "Conceptualizing and exploring bivariate spatial dependence
#       and heterogeneity: A bivariate ESDA-LISA research framework", under review.
# Date: 2017. 3. 16.
# Author: Sang-Il Lee (with help of Yongwan Chun, Hyeongmo Koo, and Monghyeon Lee)

global.lee.S <- function(x, nb, style="W", diag.zero=FALSE)
{

  # Purpose: Calculating global Lee's S
  # Arguments: 
  #   x: a vector of one variable
  #   nb: nb object
  #   style: style of spatial weight matrix (refer to nb2listw)
  #   diag.zero: if FALSE, each region has itself as its neighbour
  # Fuction called:
  #   nb.self

  style <- match.arg(style, c("B","C","S","W","U","V"))
  mf <- match.call(expand.dots = FALSE)

  # making non-zero diagonal elements in a spatial weight matrix
  if (style=="V") diag.zero <- FALSE
  if (diag.zero == FALSE) nb <- nb.self(nb)
  if (style=="V") {listw <- nb2listv(nb,beta=0.5)}
  else {listw <- nb2listw(nb,style=style)}

  V <- as(as(as_dgRMatrix_listw(listw), "RsparseMatrix"), "CsparseMatrix")
  nw <- sum(rowSums(V)^2)
  n <- length(x)
  d.x <- x - mean(x)
  z.x <- as.matrix(d.x/(sqrt(sum(d.x^2)/n)))
  lx <- lag.listw(listw,d.x)
  lee.S <- (n/nw)*sum(lx^2)/sum((d.x)^2)
  return(lee.S)
}

global.moran <- function(x, nb, style="W", diag.zero=FALSE)
{
  # Purpose: Calculating global Moran's I
  # Arguments: 
  #   x: a vector of one variable
  #   nb: nb object
  #   style: style of spatial weight matrix (refer to nb2listw)
  #   diag.zero: if FALSE, each region has itself as its neighbour
  # Fuction called:
  #   nb.self

  style <- match.arg(style, c("B","C","S","W","U","V"))
  mf <- match.call(expand.dots = FALSE)

  if (style=="V") diag.zero <- FALSE
  if (diag.zero == FALSE) nb <- nb.self(nb)
  if (style=="V") {listw <- nb2listv(nb,beta=0.5)}
  else {listw <- nb2listw(nb,style=style)}

  V <- as(as(as_dgRMatrix_listw(listw), "RsparseMatrix"), "CsparseMatrix")
  nw <- sum(rowSums(V))
  n <- length(x)
  d.x <- x - mean(x)
  z.x <- d.x/(sqrt(sum(d.x^2)/n))
  lx <- lag.listw(listw,d.x)
  moran.I <- (n/nw)*sum(d.x*lx)/sum((d.x)^2)
  return(moran.I)
}

global.geary <- function(x, nb, style="W", diag.zero=FALSE)
{
  # Purpose: Calculating global Geary's c
  # Arguments: 
  #   x: a vector of one variable
  #   nb: nb object
  #   style: style of spatial weight matrix (refer to nb2listw)
  #   diag.zero: if FALSE, each region has itself as its neighbour
  # Fuction called:
  #   nb.self

  style <- match.arg(style, c("B","C","S","W","U","V"))
  mf <- match.call(expand.dots = FALSE)

  if (style=="V") diag.zero <- FALSE
  if (diag.zero == FALSE) nb <- nb.self(nb)
  if (style=="V") {listw <- nb2listv(nb,beta=0.5)}
  else {listw <- nb2listw(nb,style=style)}
  sn <- listw2sn(listw)

  V <- as(as(as_dgRMatrix_listw(listw), "RsparseMatrix"), "CsparseMatrix")
  nw <- sum(rowSums(V))
  n <- length(x)
  d.x <- x - mean(x)
  z.x <- d.x/(sqrt(sum(d.x^2)/n))
  geary.C <- ((n-1)/(2*nw))*sum(sn[,3]*(x[sn[,1]]-x[sn[,2]])^2)/sum((d.x)^2)
  return(geary.C)
}

local.lee.S <- function (x, id, nb, style="W", diag.zero=FALSE){
  # purpose: Calculating local Lee's Si 
  # Arguments:
  #   x: a vector of one variable
  #   nb: nb object
  #   style: style of spatial weight matrix (refer to nb2listw)
  #   diag.zero: if FALSE, each region has itself as its neighbour
  # Fuction called:
  #   nb.self

  style <- match.arg(style, c("B","C","S","W","U","V"))
  mf <- match.call(expand.dots = FALSE)

  n <- length(x)

  # making non-zero diagonal elements in a spatial weight matrix
  if (style=="V") diag.zero <- FALSE
  if (diag.zero == FALSE) nb <- nb.self(nb)
  if (style=="V") {listw <- nb2listv(nb,beta=0.5)}
  else {listw <- nb2listw(nb,style=style)}

  row.sum <- unlist(lapply(listw$weights,sum))
  sum.VtV <- sum(row.sum^2)

  # dealing with diagonal elements
  vii.vec <- rep(0,n)
  if (diag.zero == FALSE) {
    iis <- unlist(lapply(1:n, function (x,nbs) match(x,nbs[[x]]), nbs=listw$neighbours))
    for (i in 1:n) vii.vec[i] <- listw$weights[[i]][iis[i]]
  }

	V <- as(as(as_dgRMatrix_listw(listw), "RsparseMatrix"), "CsparseMatrix")
	one.vec <- rep(1,n)
	M <- diag(n)-(one.vec%*%solve(crossprod(one.vec))%*%t(one.vec))

  	mu.x <- mean(x)
	sd.x <- sqrt(sum((x - mu.x)^2)/n)
	z.x <- (x-mu.x)/sd.x
	K <- n/sum.VtV
	z.x.ij.sum.vec <- numeric(n)
	local.s.vec <- numeric(n)
	for (i in 1:n){
		vi.vec <- listw$weights[[i]]
		vi.nhbr.vec <- listw$neighbours[[i]]
		z.x.j <- z.x[vi.nhbr.vec]
		z.x.ij.sum <- sum(z.x.j*vi.vec)
		local.s <- K*(z.x.ij.sum^2)
            z.x.ij.sum.vec[i] <- z.x.ij.sum
		local.s.vec[i] <- local.s
      }
	z.x.ij.sum.vec <- sqrt(K)*z.x.ij.sum.vec
	class.vec <- character(n)
	for (q in 1:n){
		if (z.x.ij.sum.vec[q] < 0)
			class.vec[q] <- "Low"
		else class.vec[q] <- "High"
	}
      res.local.s <- data.frame(id=id, local.lee=local.s.vec, z.x=z.x, v.z.x=z.x.ij.sum.vec, quad=class.vec)
      return(res.local.s)
}

local.moran <- function (x, id, nb, style="W", diag.zero=TRUE){
  # purpose: Calculating local Moran's Ii 
  #   x: a vector of one variable
  #   nb: nb object
  #   style: style of spatial weight matrix (refer to nb2listw)
  #   diag.zero: if FALSE, each region has itself as its neighbour
  # Fuction called:
  #   nb.self

  style <- match.arg(style, c("B","C","S","W","U","V"))
  mf <- match.call(expand.dots = FALSE)

  n <- length(x)

  if (style=="V") diag.zero <- FALSE
  if (diag.zero == FALSE) nb <- nb.self(nb)
  if (style=="V") {listw <- nb2listv(nb,beta=0.5)}
  else {listw <- nb2listw(nb,style=style)}

  row.sum <- unlist(lapply(listw$weights,sum))
  sum.V <- sum(row.sum)

  vii.vec <- rep(0,n)
  if (diag.zero == FALSE) {
    iis <- unlist(lapply(1:n, function (x,nbs) match(x,nbs[[x]]), nbs=listw$neighbours))
    for (i in 1:n) vii.vec[i] <- listw$weights[[i]][iis[i]]
  }

	V <- as(as(as_dgRMatrix_listw(listw), "RsparseMatrix"), "CsparseMatrix")
	one.vec <- rep(1,n)
	M <- diag(n)-(one.vec%*%solve(crossprod(one.vec))%*%t(one.vec))

  	mu.x <- mean(x)
	sd.x <- sqrt(sum((x - mu.x)^2)/n)
	z.x <- (x-mu.x)/sd.x
	z.x.2 <- z.x^2
	K <- n/sum.V
      v.z.x.vec <- numeric(n)
	local.moran.vec <- numeric(n)
	for (i in 1:n){
		vi.vec <- listw$weights[[i]]
		vi.nhbr.vec <- listw$neighbours[[i]]
            z.x.i <- z.x[i]
		z.x.j <- z.x[vi.nhbr.vec]
		v.z.x <- sum(z.x.j*vi.vec)
		local.moran.i <- K*sum(z.x.i*v.z.x)
            v.z.x.vec[i] <- v.z.x
		local.moran.vec[i] <- local.moran.i
     }
	v.z.x.vec <- K*v.z.x.vec
	class.vec <- character(n)
	for (q in 1:n){
		if (z.x[q] >= 0 & v.z.x.vec[q] >= 0) {
      	class.vec[q] <- "High-High"}
		else if (z.x[q] < 0 & v.z.x.vec[q] >= 0) {
	  		class.vec[q] <- "Low-High"}
		else if (z.x[q] < 0 & v.z.x.vec[q] < 0) {
			  class.vec[q] <- "Low-Low"}
  	else if (z.x[q] >= 0 & v.z.x.vec[q] < 0) {
	  		class.vec[q] <- "High-Low"}
	}

     res.local.moran <- data.frame(id=id, local.moran=local.moran.vec, z.x=z.x, v.z.x=v.z.x.vec, quad=class.vec)
     return(res.local.moran)
}

local.geary <- function (x, id, nb, style="W", diag.zero=TRUE){
  # purpose: Calculating local Geary's ci 
  # Arguments:
  #   x: a vector of one variable
  #   nb: nb object
  #   style: style of spatial weight matrix (refer to nb2listw)
  #   diag.zero: if FALSE, each region has itself as its neighbour
  # Fuction called:
  #   nb.self

  style <- match.arg(style, c("B","C","S","W","U","V"))
  mf <- match.call(expand.dots = FALSE)

  n <- length(x)

  if (style=="V") diag.zero <- FALSE
  if (diag.zero == FALSE) nb <- nb.self(nb)
  if (style=="V") {listw <- nb2listv(nb,beta=0.5)}
  else {listw <- nb2listw(nb,style=style)}

  row.sum <- unlist(lapply(listw$weights,sum))
  sum.V <- sum(row.sum)

  vii.vec <- rep(0,n)
  if (diag.zero == FALSE) {
    iis <- unlist(lapply(1:n, function (x,nbs) match(x,nbs[[x]]), nbs=listw$neighbours))
    for (i in 1:n) vii.vec[i] <- listw$weights[[i]][iis[i]]
  }

	V <- as(as(as_dgRMatrix_listw(listw), "RsparseMatrix"), "CsparseMatrix")
	one.vec <- rep(1,n)
	M <- diag(n)-(one.vec%*%solve(crossprod(one.vec))%*%t(one.vec))

  	mu.x <- mean(x)
	sd.x <- sqrt(sum((x - mu.x)^2)/n)
	z.x <- (x-mu.x)/sd.x
	K <- (n-1)/(2*sum.V)
	local.geary.vec <- numeric(n)
	for (i in 1:n){
		vi.vec <- listw$weights[[i]]
		vi.nhbr.vec <- listw$neighbours[[i]]
            z.x.i <- z.x[i]
		z.x.j <- z.x[vi.nhbr.vec]
		local.geary.i <- K*sum(((z.x.i-z.x.j)^2)*vi.vec)
		local.geary.vec[i] <- local.geary.i
     }
	class.vec <- vector()
	for (q in 1:n)
	{
		if (z.x[q] < 0)
			class.vec[q] <- as.character("Low")
		else class.vec[q] <- as.character("High")
	}

     res.local.geary <- data.frame(id=id, local.geary=local.geary.vec, z.x=z.x, quad=class.vec)
     return(res.local.geary)
}

local.getis_ord <- function (x, id, nb, style="W", diag.zero=FALSE)
{
  # purpose: Calculating local Getic-Ord's Gi and Gi*
  # Arguments:
  #   x: a vector of one variable
  #   nb: nb object
  #   style: style of spatial weight matrix (refer to nb2listw)
  #   diag.zero: if FALSE, each region has itself as its neighbour
  # Fuction called:
  #   nb.self
  style <- match.arg(style, c("B","C","S","W","U","V"))
  mf <- match.call(expand.dots = FALSE)

  n <- length(x)

  # making non-zero diagonal elements in a spatial weight matrix
  if (style=="V") diag.zero <- FALSE
  if (diag.zero == FALSE) nb <- nb.self(nb)
  if (style=="V") {listw <- nb2listv(nb,beta=0.5)}
  else {listw <- nb2listw(nb,style=style)}

  row.sum <- unlist(lapply(listw$weights,sum))
  sum.V <- sum(row.sum)

  nhbr <- listw2sn(listw)

  sd.x <- sqrt(sum((x-mean(x))^2)/n)
  z.x <- (x-mean(x))/sd.x
  x.j <- x[nhbr[,2]]	
  v.j <- nhbr[,3]
  v.j.2 <- v.j^2
  x.v.j <- x.j*v.j
  sum.x.v.j <- as.vector(tapply(x.v.j, nhbr[,1], sum))
  sum.v.j <- as.vector(tapply(v.j, nhbr[,1], sum))
  sum.v.j.2 <- as.vector(tapply(v.j.2, nhbr[,1], sum))
  sum.v.j.mu.x <- sum.v.j*mean(x)
  g.vec <- (sum.x.v.j-sum.v.j.mu.x)/(sd.x*sqrt((n*sum.v.j.2-sum.v.j^2)/(n-1)))
  class.vec <- vector()
  for (q in 1:n)
  {
	if (g.vec[q] < 0)
  	      class.vec[q] <- as.character("Low")
		else class.vec[q] <- as.character("High")
  }

  res.g <- data.frame(id=id, local.g=g.vec, z.x=z.x, quad=class.vec)
  return(res.g)
}

test.global.lee.S <- function(x, nb, style="W", method="randomization", alternative="two.sided", diag.zero=FALSE)
{

  # Purpose: Conducting significance testing for global Lee's S
  # Arguments: 
  #   x: a vector of one variable
  #   nb: nb object
  #   style: style of spatial weight matrix (refer to nb2listw)
  #   method: "normality" or "randomization" assumption for null hypothesis 
  #   alternative: type of test
  #   diag.zero: if FALSE, each region has itself as its neighbour
  # Fuction called:
  #   nb.self

  alternative <- match.arg(alternative, c("greater", "less", "two.sided"))
  style <- match.arg(style, c("B","C","S","W","U","V"))
  method <- match.arg(method, c("normality", "randomization"))
  mf <- match.call(expand.dots = FALSE)

  # making non-zero diagonal elements in a spatial weight matrix
  if (style=="V") diag.zero <- FALSE
  if (diag.zero == FALSE) nb <- nb.self(nb)
  if (style=="V") {listw <- nb2listv(nb,beta=0.5)}
  else {listw <- nb2listw(nb,style=style)}

  V <- as(as(as_dgRMatrix_listw(listw), "RsparseMatrix"), "CsparseMatrix")
  nw <- sum(rowSums(V)^2)
  n <- length(x)
  d.x <- x - mean(x)
  z.x <- as.matrix(d.x/(sqrt(sum(d.x^2)/n)))
  lx <- lag.listw(listw,d.x)
  lee.S <- (n/nw)*sum(lx^2)/sum((d.x)^2)

  if (method=="normality")
  {
  	V <- crossprod(V)
	one.vec <- rep(1,n)

	M <- diag(n)-(one.vec%*%solve(crossprod(one.vec))%*%t(one.vec))
	V.s <- (n*V)/sum(V)
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
	Z.S <- (lee.S-E.S)/SD.S

  # Calculation p-values
	if (alternative=="two.sided")
		P.S <- 2 * pnorm(abs(Z.S), lower.tail = FALSE)
	else if (alternative=="greater")
		P.S <- pnorm(Z.S, lower.tail = FALSE)
	else P.S <- pnorm(Z.S)

  statistic <- Z.S
  attr(statistic, "names") <- "Lee's S statistic standard deviate"
  p.value <- P.S
  estimate <- c(lee.S, E.S, VAR.S)
  attr(estimate, "names") <- c("Observed Lee's S", "Expectation","Variance")
  method <- "Global Lee's S test under normality assumption"

  if (diag.zero==FALSE) style <- paste(style, "*",sep="")
  data.name <- paste(deparse(mf[[2]]), "\nweights: ",  deparse(mf[[3]]), ", style: ", style, "\n", sep = "")
  res <- list(statistic = statistic, p.value = p.value, estimate = estimate,
        method = method, alternative = alternative, data.name = data.name)
  class(res) <- "htest"
  res
  }	
	
  else if (method=="randomization")
  {
	Y <- z.x%*%t(z.x)
	V <- crossprod(V)
	V <- V/sum(V)

#	lee.S <- sum(V*Y)

	P <- 0.5*(V+t(V))
	Q <- 0.5*(Y+t(Y))

	P.off <- P
	diag(P.off) <- 0
	Q.off <- Q
	diag(Q.off) <- 0
	P.on <- diag(P)
	Q.on <- diag(Q)

	F0.off <- sum(P.off)
	F1.off <- sum(P.off^2)
	F2.off <- sum(rowSums(P.off)^2)
	F0.on <- sum(P.on)
	F1.on <- sum(P.on^2)
	F0.all <- sum(P)
	F1.all <- sum(P^2)

	F2.all <- sum(rowSums(P)^2)
	F2.on <- sum(P.on*rowSums(P))
	G0.off <- sum(Q.off)
	G1.off <- sum(Q.off^2)
	G2.off <- sum(rowSums(Q.off)^2)
	G0.on <- sum(Q.on)
	G1.on <- sum(Q.on^2)
	G2.on <- sum(Q.on*rowSums(Q))
	G0.all <- sum(Q)
	G1.all <- sum(Q^2)
	G2.all <- sum(rowSums(Q)^2)

	E.S.off <- (F0.off*G0.off)/(n*(n-1))
	E.S.on <- (F0.on*G0.on)/n
	E.S <- E.S.off + E.S.on

	VAR.S.off <- ((2*F1.off*G1.off)/(n*(n-1)))+((4*(F2.off-F1.off)*(G2.off-G1.off))/(n*(n-1)*(n-2)))+ ((((F0.off^2)+(2*F1.off)-(4*F2.off))*((G0.off^2)+(2*G1.off)-(4*G2.off)))/(n*(n-1)*(n-2)*(n-3)))-(E.S.off^2)
	VAR.S.on <- (F1.on*G1.on/n)+(((F0.on^2-F1.on)*(G0.on^2-G1.on))/(n*(n-1)))-(E.S.on^2)
	COV.1 <- ((F2.all-F1.on-F2.off)*(G2.all-G1.on-G2.off))/(2*n*(n-1))
	COV.2 <- (((F0.on*F0.off)-(F2.all-F1.on-F2.off))*((G0.on*G0.off)-(G2.all-G1.on-G2.off)))/(n*(n-1)*(n-2))
	COV.S <- (COV.1+COV.2)-(E.S.off*E.S.on)
	COV.S.2 <- 2*COV.S

	VAR.S <- VAR.S.off + VAR.S.on + COV.S.2

	SD.S <- sqrt(VAR.S)
	Z.S <- (lee.S - E.S)/SD.S

  # Calculation p-values
	if (alternative=="two.sided")
		P.S <- 2 * pnorm(abs(Z.S), lower.tail = FALSE)
	else if (alternative=="greater")
		P.S <- pnorm(Z.S, lower.tail = FALSE)
	else P.S <- pnorm(Z.S)

  statistic <- Z.S
  attr(statistic, "names") <- "Lee's S statistic standard deviate"
  p.value <- P.S
  estimate <- c(lee.S, E.S, VAR.S)
  attr(estimate, "names") <- c("Observed Lee's S", "Expectation","Variance")
  method <- "Global Lee's S test under randomization assumption"

  if (diag.zero==FALSE) style <- paste(style, "*",sep="")
  data.name <- paste(deparse(mf[[2]]), "\nweights: ",  deparse(mf[[3]]), ", style: ", style, "\n", sep = "")
  res <- list(statistic = statistic, p.value = p.value, estimate = estimate,
        method = method, alternative = alternative, data.name = data.name)
  class(res) <- "htest"
  res
  }
}

test.global.z2 <- function(x, alternative="two.sided")
{
  # Purpose: Conducting significance testing for global z^2
  # Arguments: 
  #   x: a vector of one variable
  #   alternative: type of test

  alternative <- match.arg(alternative, c("greater", "less", "two.sided"))
  mf <- match.call(expand.dots = FALSE)

	n <- length(x)
	mu.x <- mean(x)	
	sd.x <- sqrt(((n-1)/n)*var(x))
	z.x <- (x-mu.x)/sd.x
	
	global.z2 <- sum(z.x^2)/n
	
	E.z2 <- 0
	VAR.z2 <- 1/(n-1)
	SD.z2 <- sqrt(VAR.z2)
	
	Z.z2 <- (global.z2 - E.z2)/SD.z2

  # Calculation p-values
	if (alternative=="two.sided")
		P.z2 <- 2 * pnorm(abs(Z.z2), lower.tail = FALSE)
	else if (alternative=="greater")
		P.z2 <- pnorm(Z.z2, lower.tail = FALSE)
	else P.z2 <- pnorm(Z.z2)

  statistic <- Z.z2
  attr(statistic, "names") <- "Global z^2 statistic standard deviate"
  p.value <- P.z2
  estimate <- c(global.z2, E.z2, VAR.z2)
  attr(estimate, "names") <- c("Observed Global z^2", "Expectation","Variance")
  method <- "Global z^2 test under randomization assumption"

# if (diag.zero==FALSE) style <- paste(style, "*",sep="")
# data.name <- paste(deparse(mf[[2]]), "\nweights: ",  deparse(mf[[4]]), ", style: ", style, "\n", sep = "")
  data.name <- deparse(mf[[2]])
  res <- list(statistic = statistic, p.value = p.value, estimate = estimate,
        method = method, alternative = alternative, data.name = data.name)
  class(res) <- "htest"
  res
}

test.global.moran <- function(x, nb, style="W", method="randomization", alternative="two.sided", diag.zero=FALSE)
{

  # Purpose: Conducting significance testing for global Moran's I
  # Arguments: 
  #   x: a vector of one variable
  #   nb: nb object
  #   style: style of spatial weight matrix (refer to nb2listw)
  #   method: "normality" or "randomization" assumption for null hypothesis 
  #   alternative: type of test
  #   diag.zero: if FALSE, each region has itself as its neighbour
  # Fuction called:
  #   nb.self

  alternative <- match.arg(alternative, c("greater", "less", "two.sided"))
  style <- match.arg(style, c("B","C","S","W","U","V"))
  method <- match.arg(method, c("normality", "randomization"))
  mf <- match.call(expand.dots = FALSE)

  # making non-zero diagonal elements in a spatial weight matrix
  if (style=="V") diag.zero <- FALSE
  if (diag.zero == FALSE) nb <- nb.self(nb)
  if (style=="V") {listw <- nb2listv(nb,beta=0.5)}
  else {listw <- nb2listw(nb,style=style)}
# sn <- listw2sn(listw)

  V <- as(as(as_dgRMatrix_listw(listw), "RsparseMatrix"), "CsparseMatrix")
  nw <- sum(rowSums(V))
  n <- length(x)
  d.x <- x - mean(x)
  z.x <- d.x/(sqrt(sum(d.x^2)/n))
  lx <- lag.listw(listw,d.x)
  moran.I <- (n/nw)*sum(d.x*lx)/sum((d.x)^2)

	if (method=="normality")
	{
		V.s <- 0.5*(V+t(V))
		one.vec <- rep(1,n)
		M <- diag(n)-(one.vec%*%solve(crossprod(one.vec))%*%t(one.vec))
		V.s <- (n*V.s)/sum(V)
		K <- M%*%V.s

		tr <- function (a)
		{
			sum(diag(a))
		}

		mu.1 <- tr(K)/(n-1)
		mu.2 <- 2*(((n-1)*tr(K%*%K))-(tr(K)^2))/(((n-1)^2)*(n+1))
		mu.3 <- 8*(((n-1)^2*tr(K%*%K%*%K))-(3*(n-1)*tr(K)*tr(K%*%K))+(2*(tr(K)^3)))/((n-1)^3*(n+1)*(n+3))
		mu.4 <- 12*((((n-1)^3)*(4*tr(K%*%K%*%K%*%K)+(tr(K%*%K)^2)))-((2*((n-1)^2))*((8*tr(K)*tr(K%*%K%*%K))+(tr(K%*%K)*(tr(K)^2))))+((n-1)*((24*tr(K%*%K)*(tr(K)^2))+(tr(K)^4)))-(12*(tr(K)^4)))/(((n-1)^4)*(n+1)*(n+3)*(n+5))
	
		E.I <- mu.1
		VAR.I <- mu.2
		SD.I <- sqrt(mu.2)
		SKEW.I <- mu.3/((mu.2)^(3/2))
		KURT.I <- mu.4/((mu.2)^2)
		Z.I <- (moran.I-E.I)/SD.I

  # Calculation p-values
	if (alternative=="two.sided")
		P.I <- 2 * pnorm(abs(Z.I), lower.tail = FALSE)
	else if (alternative=="greater")
		P.I <- pnorm(Z.I, lower.tail = FALSE)
	else P.I <- pnorm(Z.I)

  statistic <- Z.I
  attr(statistic, "names") <- "Moran's I statistic standard deviate"
  p.value <- P.I
  estimate <- c(moran.I, E.I, VAR.I, SKEW.I, KURT.I)
  attr(estimate, "names") <- c("Observed Moran's I", "Expectation","Variance", "Skewness", "Kurtosis")
  method <- "Global Moran's I test under normality assumption"

  if (diag.zero==FALSE) style <- paste(style, "*",sep="")
  data.name <- paste(deparse(mf[[2]]), "\nweights: ",  deparse(mf[[3]]), ", style: ", style, "\n", sep = "")
  res <- list(statistic = statistic, p.value = p.value, estimate = estimate,
        method = method, alternative = alternative, data.name = data.name)
  class(res) <- "htest"
  res
	}

	else if (method=="randomization")
	{
		V <- V/sum(V)
		Y <- z.x%*%t(z.x)
		P <- 0.5*(V+t(V))
		Q <- 0.5*(Y+t(Y))
		P.off <- P
		diag(P.off) <- 0
		Q.off <- Q
		diag(Q.off) <- 0
		P.on <- diag(P)
		Q.on <- diag(Q)

		F0.off <- sum(P.off)
		F1.off <- sum(P.off^2)
		F2.off <- sum(rowSums(P.off)^2)
		F0.on <- sum(P.on)
		F1.on <- sum(P.on^2)
		F0.all <- sum(P)
		F1.all <- sum(P^2)
		F2.all <- sum(rowSums(P)^2)
		F2.on <- sum(P.on*rowSums(P))
		G0.off <- sum(Q.off)
		G1.off <- sum(Q.off^2)
		G2.off <- sum(rowSums(Q.off)^2)
		G0.on <- sum(Q.on)
		G1.on <- sum(Q.on^2)
		G2.on <- sum(Q.on*rowSums(Q))
		G0.all <- sum(Q)
		G1.all <- sum(Q^2)
		G2.all <- sum(rowSums(Q)^2)

		E.I.off <- (F0.off*G0.off)/(n*(n-1))
		E.I.on <- (F0.on*G0.on)/n
		E.I <- E.I.off + E.I.on

		VAR.I.off <- ((2*F1.off*G1.off)/(n*(n-1)))+((4*(F2.off-F1.off)*(G2.off-G1.off))/(n*(n-1)*(n-2)))+ ((((F0.off^2)+(2*F1.off)-(4*F2.off))*((G0.off^2)+(2*G1.off)-(4*G2.off)))/(n*(n-1)*(n-2)*(n-3)))-(E.I.off^2)
		VAR.I.on <- (F1.on*G1.on/n)+(((F0.on^2-F1.on)*(G0.on^2-G1.on))/(n*(n-1)))-(E.I.on^2)
		COV.1 <- ((F2.all-F1.on-F2.off)*(G2.all-G1.on-G2.off))/(2*n*(n-1))
		COV.2 <- (((F0.on*F0.off)-(F2.all-F1.on-F2.off))*((G0.on*G0.off)-(G2.all-G1.on-G2.off)))/(n*(n-1)*(n-2))
		COV.I <- (COV.1+COV.2)-(E.I.off*E.I.on)
		COV.I.2 <- 2*COV.I
		VAR.I <- VAR.I.off + VAR.I.on + COV.I.2

		SD.I <- sqrt(VAR.I)
		Z.I <- (moran.I - E.I)/SD.I

  # Calculation p-values
	if (alternative=="two.sided")
		P.I <- 2 * pnorm(abs(Z.I), lower.tail = FALSE)
	else if (alternative=="greater")
		P.I <- pnorm(Z.I, lower.tail = FALSE)
	else P.I <- pnorm(Z.I)

  statistic <- Z.I
  attr(statistic, "names") <- "Moran's I statistic standard deviate"
  p.value <- P.I
  estimate <- c(moran.I, E.I, VAR.I)
  attr(estimate, "names") <- c("Observed Moran's I", "Expectation","Variance")
  method <- "Global Moran's I test under randomization assumption"

  if (diag.zero==FALSE) style <- paste(style, "*",sep="")
  data.name <- paste(deparse(mf[[2]]), "\nweights: ",  deparse(mf[[3]]), ", style: ", style, "\n", sep = "")
  res <- list(statistic = statistic, p.value = p.value, estimate = estimate,
        method = method, alternative = alternative, data.name = data.name)
  class(res) <- "htest"
  res
	}
}

test.global.geary <- function(x, nb, style="W", method="randomization", alternative="two.sided", diag.zero=FALSE)
{

  # Purpose: Conducting significance testing for global Geary's c
  # Arguments: 
  #   x: a vector of one variable
  #   nb: nb object
  #   style: style of spatial weight matrix (refer to nb2listw)
  #   method: "normality" or "randomization" assumption for null hypothesis 
  #   alternative: type of test
  #   diag.zero: if FALSE, each region has itself as its neighbour
  # Fuction called:
  #   nb.self

  alternative <- match.arg(alternative, c("greater", "less", "two.sided"))
  style <- match.arg(style, c("B","C","S","W","U","V"))
  method <- match.arg(method, c("normality", "randomization"))
  mf <- match.call(expand.dots = FALSE)

  # making non-zero diagonal elements in a spatial weight matrix
  if (style=="V") diag.zero <- FALSE
  if (diag.zero == FALSE) nb <- nb.self(nb)
  if (style=="V") {listw <- nb2listv(nb,beta=0.5)}
  else {listw <- nb2listw(nb,style=style)}
  sn <- listw2sn(listw)

  V <- as(as(as_dgRMatrix_listw(listw), "RsparseMatrix"), "CsparseMatrix")
  nw <- sum(rowSums(V))
  n <- length(x)
  d.x <- x - mean(x)
  z.x <- d.x/(sqrt(sum(d.x^2)/n))
  geary.C <- ((n-1)/(2*nw))*sum(sn[,3]*(x[sn[,1]]-x[sn[,2]])^2)/sum((d.x)^2)

	if (method=="normality")
	{
		sum.V <- sum(V)
		sV <- 0.5*(V+t(V))
		row.sum <- rowSums(sV)
		Omega.matrix <- V*0
		diag(Omega.matrix) <- row.sum
		Omega.sV <- (Omega.matrix - sV)

		one.vec <- rep(1, n)
		M <- diag(n)-(one.vec%*%solve(crossprod(one.vec))%*%t(one.vec))
		V.s <- (n-1)*Omega.sV/sum(V)
		K <- M%*%V.s
	
		tr <- function (a)
		{
			sum(diag(a))
		}

		mu.1 <- tr(K)/(n-1)
		mu.2 <- 2*(((n-1)*tr(K%*%K))-(tr(K)^2))/(((n-1)^2)*(n+1))
		mu.3 <- 8*(((n-1)^2*tr(K%*%K%*%K))-(3*(n-1)*tr(K)*tr(K%*%K))+(2*(tr(K)^3)))/((n-1)^3*(n+1)*(n+3))
		mu.4 <- 12*((((n-1)^3)*(4*tr(K%*%K%*%K%*%K)+(tr(K%*%K)^2)))-((2*((n-1)^2))*((8*tr(K)*tr(K%*%K%*%K))+(tr(K%*%K)*(tr(K)^2))))+((n-1)*((24*tr(K%*%K)*(tr(K)^2))+(tr(K)^4)))-(12*(tr(K)^4)))/(((n-1)^4)*(n+1)*(n+3)*(n+5))
	
		E.C <- mu.1
		VAR.C <- mu.2
		SD.C <- sqrt(mu.2)
		SKEW.C <- mu.3/((mu.2)^(3/2))
		KURT.C <- mu.4/((mu.2)^2)
		Z.C <- (geary.C-E.C)/SD.C

  # Calculation p-values
	if (alternative=="two.sided")
		P.C <- 2 * pnorm(abs(Z.C), lower.tail = FALSE)
	else if (alternative=="greater")
		P.C <- pnorm(Z.C, lower.tail = FALSE)
	else P.C <- pnorm(Z.C)

  statistic <- Z.C
  attr(statistic, "names") <- "Geary's c statistic standard deviate"
  p.value <- P.C
  estimate <- c(geary.C, E.C, VAR.C, SKEW.C, KURT.C)
  attr(estimate, "names") <- c("Observed Geary's c", "Expectation","Variance", "Skewness", "Kurtosis")
  method <- "Global Geary's c test under normality assumption"

  if (diag.zero==FALSE) style <- paste(style, "*",sep="")
  data.name <- paste(deparse(mf[[2]]), "\nweights: ",  deparse(mf[[3]]), ", style: ", style, "\n", sep = "")
  res <- list(statistic = statistic, p.value = p.value, estimate = estimate,
        method = method, alternative = alternative, data.name = data.name)
  class(res) <- "htest"
  res

	}

	else if (method=="randomization")
	{
		XX <- z.x%*%t(z.x)
		sum.V <- sum(V)
		sV <- 0.5*(V+t(V))
		row.sum <- rowSums(sV)
		Omega.matrix <- V*0
		diag(Omega.matrix) <- row.sum
		Omega.sV <- (Omega.matrix - sV)
		Omega.sV <- ((n-1)/n)*(Omega.sV/sum.V)
		P <- Omega.sV
		Q <- XX
		P.off <- P
		diag(P.off) <- 0
		Q.off <- Q
		diag(Q.off) <- 0
		P.on <- diag(P)
		Q.on <- diag(Q)
		F0.off <- sum(P.off)
		F1.off <- sum(P.off^2)
		F2.off <- sum(rowSums(P.off)^2)
		F0.on <- sum(P.on)
		F1.on <- sum(P.on^2)
		F0.all <- sum(P)
		F1.all <- sum(P^2)
		F2.all <- sum(rowSums(P)^2)
		F2.on <- sum(P.on*rowSums(P))
		G0.off <- sum(Q.off)
		G1.off <- sum(Q.off^2)
		G2.off <- sum(rowSums(Q.off)^2)
		G0.on <- sum(Q.on)
		G1.on <- sum(Q.on^2)
		G2.on <- sum(Q.on*rowSums(Q))
		G0.all <- sum(Q)
		G1.all <- sum(Q^2)
		G2.all <- sum(rowSums(Q)^2)

		E.C.off <- (F0.off*G0.off)/(n*(n-1))
		E.C.on <- (F0.on*G0.on)/n
		E.C <- E.C.off + E.C.on

		VAR.C.off <- ((2*F1.off*G1.off)/(n*(n-1)))+((4*(F2.off-F1.off)*(G2.off-G1.off))/(n*(n-1)*(n-2)))+ ((((F0.off^2)+(2*F1.off)-(4*F2.off))*((G0.off^2)+(2*G1.off)-(4*G2.off)))/(n*(n-1)*(n-2)*(n-3)))-(E.C.off^2)
		VAR.C.on <- (F1.on*G1.on/n)+(((F0.on^2-F1.on)*(G0.on^2-G1.on))/(n*(n-1)))-(E.C.on^2)
		COV.1 <- ((F2.all-F1.on-F2.off)*(G2.all-G1.on-G2.off))/(2*n*(n-1))
		COV.2 <- (((F0.on*F0.off)-(F2.all-F1.on-F2.off))*((G0.on*G0.off)-(G2.all-G1.on-G2.off)))/(n*(n-1)*(n-2))
		COV.C <- (COV.1+COV.2)-(E.C.off*E.C.on)
		COV.C.2 <- 2*COV.C

		VAR.C <- VAR.C.off + VAR.C.on + COV.C.2

		SD.C <- sqrt(VAR.C)
		Z.C <- (geary.C - E.C)/SD.C

  # Calculation p-values
	if (alternative=="two.sided")
		P.C <- 2 * pnorm(abs(Z.C), lower.tail = FALSE)
	else if (alternative=="greater")
		P.C <- pnorm(Z.C, lower.tail = FALSE)
	else P.C <- pnorm(Z.C)

  statistic <- Z.C
  attr(statistic, "names") <- "Geary's c statistic standard deviate"
  p.value <- P.C
  estimate <- c(geary.C, E.C, VAR.C)
  attr(estimate, "names") <- c("Observed Geary's c", "Expectation","Variance")
  method <- "Global Geary's c test under randomization assumption"

  if (diag.zero==FALSE) style <- paste(style, "*",sep="")
  data.name <- paste(deparse(mf[[2]]), "\nweights: ",  deparse(mf[[3]]), ", style: ", style, "\n", sep = "")
  res <- list(statistic = statistic, p.value = p.value, estimate = estimate,
        method = method, alternative = alternative, data.name = data.name)
  class(res) <- "htest"
  res
	}
}

test.local.lee.S <- function (x, id, nb, style="W", sig.levels=c(0.05), method="total", alternative="two.sided", diag.zero=FALSE){
  # purpose: Calculating local Lee's Si and conducting significance testing
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
	VAR.S.vec <- numeric(n)
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
		} 
		else {
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
		else if (method=="normality") {
		V.i <- matrix(rep(0, n^2), ncol=n)
		V.i[i,] <- V[i,]
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
	  VAR.S.vec[i] <- VAR.S
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
	z.x.ij.sum.vec <- sqrt(K)*z.x.ij.sum.vec
	class.vec <- character(n)
	for (q in 1:n){
		if (z.x.ij.sum.vec[q] < 0)
			class.vec[q] <- "Low"
		else class.vec[q] <- "High"
	}
 
  # Significance for different levels
	sig.df <- as.data.frame(sapply(sig.levels, function(x, p.l, c.d) {ifelse(p.l > x, "not sig.", c.d)}, p.l=P.S.vec, c.d=class.vec))
	colnames(sig.df) <- paste("sig.quad.", sig.levels, sep="")

  # To make a composite vector of quad and the order of significance
	n.sig <- length(sig.levels)
	sig.1 <- rev(sort(sig.levels))[1]
	final.quad.sig <- ifelse (P.S.vec <= sig.1, paste(class.vec, n.sig, sep=""), as.character("not sig."))
	k <- 2
	while (k <= n.sig)
	{
		sig.k <- rev(sort(sig.levels))[k]
		final.quad.sig[P.S.vec <= sig.k] <- paste(class.vec[P.S.vec <= sig.k], n.sig+1-k, sep="")
		k <- k+1
	}
  #	
	local.s.res <- data.frame(id=id, lee.S=local.s.vec, z.x=z.x, v.z.x=z.x.ij.sum.vec, EXP=E.S.vec, VAR=VAR.S.vec,
                 SD=SD.S.vec, Z=Z.S.vec, P=P.S.vec, quad=class.vec, sig.df, final.quad=final.quad.sig)
	return(local.s.res)
}

test.local.z2 <- function (x, id, sig.levels=c(0.05), method="randomization", alternative="two.sided"){
  # purpose: Calculating local Pearson's ri and conducting significance testing
  # Arguments:
  #   x: a vector of one variable
  #   id: a vector of ID codes
  #   sig: a vector of significance levels	
  #   method: "randomization" or "normality" assumption for null hypothesis
  #   alternatie: type of test
  #   diag.zero: if FALSE, each region has itself as its neighbour

  alternative <- match.arg(alternative, c("greater", "less", "two.sided"))
#  mf <- match.call(expand.dots = FALSE)

  n <- length(x)

	mu.x <- mean(x)
	sd.x <- sqrt(sum((x - mu.x)^2)/n)
	z.x <- (x-mu.x)/sd.x

  # calculating a local R value

	if (method=="normality")
	{
	local.z2.vec <- z.x^2
	e.z2 <- 1
	E.z2.vec <- rep(e.z2, n)
	var.z2 <- 2
	VAR.z2.vec <- rep(var.z2, n)
	SD.z2.vec <- sqrt(2)
	skew.z2 <- 8/(2)^(3/2)
	Z.z2.vec <- (local.z2.vec - E.z2.vec)/SD.z2.vec
	}

	else if (method=="randomization")
	{
	local.z2.vec <- z.x^2
	e.z2 <- 1
	E.z2.vec <- rep(e.z2, n)
#	var.z2 <- mean(z.x^4)
	m4 <- sum((x-mean(x))^4)/n
	m2 <- sum((x-mean(x))^2)/n
	var.z2 <- (m4/m2^2)-1
	VAR.z2.vec <- rep(var.z2, n)
	SD.z2.vec <- sqrt(VAR.z2.vec)
	Z.z2.vec <- (local.z2.vec - E.z2.vec)/SD.z2.vec
	}
	
  # calculating p-values
	#P.L.vec <- numeric(n)
	if (alternative=="two.sided") {
	    P.z2.vec <- sapply(Z.z2.vec, function(x) {2 * pnorm(abs(x), lower.tail = FALSE)})}
	else if (alternative=="greater") {
   		P.z2.vec <- sapply(Z.z2.vec, function(x) {pnorm(x, lower.tail = FALSE)})}
	else {
      P.z2.vec <- sapply(Z.R.vec, function(x) {pnorm(x, lower.tail = TRUE)})}

  # an analogy of Moran scatterplot
	class.vec <- character(n)
	for (q in 1:n){
		if (z.x[q] >= 0) {
      	class.vec[q] <- "High"}
		else if (z.x[q] < 0) {
	  		class.vec[q] <- "Low"}
		}
 
  # Significance for different levels
	sig.df <- as.data.frame(sapply(sig.levels, function(x, p.l, c.d) {ifelse(p.l > x, "not sig.", c.d)}, p.l=P.z2.vec, c.d=class.vec))
	colnames(sig.df) <- paste("sig.quad.", sig.levels, sep="")

  # To make a composite vector of quad and the order of significance
	n.sig <- length(sig.levels)
	sig.1 <- rev(sort(sig.levels))[1]
	final.quad.sig <- ifelse (P.z2.vec <= sig.1, paste(class.vec, n.sig, sep=""), as.character("not sig."))
	k <- 2
	while (k <= n.sig)
	{
		sig.k <- rev(sort(sig.levels))[k]
		final.quad.sig[P.z2.vec <= sig.k] <- paste(class.vec[P.z2.vec <= sig.k], n.sig+1-k, sep="")
		k <- k+1
	}
  #	

	z2.res <- data.frame(id=id, local.z2=local.z2.vec, z.x=z.x, E=E.z2.vec, VAR=VAR.z2.vec,
                 SD=SD.z2.vec, Z=Z.z2.vec, P=P.z2.vec, quad=class.vec, sig.df, z2.final.quad=final.quad.sig)

	return(z2.res)

}

test.local.moran <- function (x, id, nb, style="W", sig.levels=c(0.05), method="total", alternative="two.sided", diag.zero=TRUE){
  # purpose: Calculating local Moran's Ii and conducting significance testing
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

	K <- n/sum.V

	m2 <- sum((x-mean(x))^2)/n
	m4 <- sum((x-mean(x))^4)/n
	b2 <- m4/m2^2

  # objects to store results

	local.moran.vec <- vector()
	E.I.vec <- vector()
	VAR.I.vec <- vector()
	v.z.x.vec <- vector()

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
		v.z.x <- sum(z.x.j*vi.vec)
		v.z.x.vec[i] <- v.z.x
  # calculating a local L value
		local.moran.i <- K*sum(z.x.i*v.z.x)
		local.moran.vec[i] <- local.moran.i

  # calculating an expectation and variance
		if (method=="conditional") {
		E.I.off <- (-1*K/(n-1))*(vi.s-vii)*(z.x.i^2)
		E.I.on <- K*vii*(z.x.i^2)
		E.I <- E.I.off + E.I.on
		VAR.I <- (K^2)*(n/((n-2)*((n-1)^2)))*((n-1)*(vi.2.s-(vii^2))-((vi.s-vii)^2))*(z.x.i^2)*(n-1-(z.x.i^2))
		}
		else if (method=="total") {
		E.I.off <- K*(vii-vi.s)/(n-1)
		E.I.on <- K*vii
		E.I <- E.I.off + E.I.on
		VAR.I.off <- ((K^2)*(((vi.2.s-vii.2)*(n-b2)/(n-1))+((((vi.2.s-vii.2)-((vi.s-vii)^2))*(n-(2*b2)))/((n-1)*(n-2)))))-(E.I.off^2)
		VAR.I.on <- (b2-1)*(K^2)*vii.2
		COV.I <- (K^2)*vii*(vii-vi.s)*(b2-1)/(n-1)
		COV.I.2 <- 2*COV.I
		VAR.I <- VAR.I.off + VAR.I.on + COV.I.2
		E.I.vec[i] <- E.I
		VAR.I.vec[i] <- VAR.I
		}
		else if (method=="normality") {
		V.i <- matrix(rep(0, n^2), ncol=n)
		V.i[i,] <- V[i,]
		V.i <- 0.5*(V.i+t(V.i))
		T.i <- (n^2)*V.i/sum(V)
		K.i <- M%*%T.i
		v.z.x <- sum(V.i%*%z.x)
		v.z.x.vec[i] <- v.z.x
		tr <- function (a)
		{
			sum(diag(a))
		}
		mu.1 <- tr(K.i)/(n-1)
		mu.2 <- 2*(((n-1)*tr(K.i%*%K.i))-(tr(K.i)^2))/(((n-1)^2)*(n+1))
		mu.3 <- 8*(((n-1)^2*tr(K.i%*%K.i%*%K.i))-(3*(n-1)*tr(K)*tr(K.i%*%K.i))+(2*(tr(K.i)^3)))/((n-1)^3*(n+1)*(n+3))
		mu.4 <- 12*((((n-1)^3)*(4*tr(K.i%*%K.i%*%K.i%*%K.i)+(tr(K.i%*%K.i)^2)))-((2*((n-1)^2))*((8*tr(K.i)*tr(K.i%*%K.i%*%K.i))+(tr(K.i%*%K.i)*(tr(K.i)^2))))+((n-1)*((24*tr(K.i%*%K.i)*(tr(K.i)^2))+(tr(K.i)^4)))-(12*(tr(K.i)^4)))/(((n-1)^4)*(n+1)*(n+3)*(n+5))
		E.I <- mu.1
		VAR.I <- mu.2
		SKEW.I <- mu.3/((mu.2)^(3/2))
		KURT.I <- mu.4/((mu.2)^2)
		}

    # store values
	  E.I.vec[i] <- E.I
	  VAR.I.vec[i] <- VAR.I
	}

	SD.I.vec <- sqrt(VAR.I.vec)
	Z.I.vec <- (local.moran.vec-E.I.vec)/SD.I.vec

  # calculating p-values
	#P.L.vec <- numeric(n)
	if (alternative=="two.sided") {
	    P.I.vec <- sapply(Z.I.vec, function(x) {2 * pnorm(abs(x), lower.tail = FALSE)})}
	else if (alternative=="greater") {
   		P.I.vec <- sapply(Z.I.vec, function(x) {pnorm(x, lower.tail = FALSE)})}
	else {
      P.I.vec <- sapply(Z.I.vec, function(x) {pnorm(x, lower.tail = TRUE)})}

  # an analogy of Moran scatterplot
	v.z.x.vec <- K*v.z.x.vec
	class.vec <- character(n)
	for (q in 1:n){
		if (z.x[q] >= 0 & v.z.x.vec[q] >= 0) {
      	class.vec[q] <- "High-High"}
		else if (z.x[q] < 0 & v.z.x.vec[q] >= 0) {
	  		class.vec[q] <- "Low-High"}
		else if (z.x[q] < 0 & v.z.x.vec[q] < 0) {
			  class.vec[q] <- "Low-Low"}
  	else if (z.x[q] >= 0 & v.z.x.vec[q] < 0) {
	  		class.vec[q] <- "High-Low"}
	}
 
  # Significance for different levels
	sig.df <- as.data.frame(sapply(sig.levels, function(x, p.l, c.d) {ifelse(p.l > x, "not sig.", c.d)}, p.l=P.I.vec, c.d=class.vec))
	colnames(sig.df) <- paste("sig.quad.", sig.levels, sep="")

  # To make a composite vector of quad and the order of significance
	n.sig <- length(sig.levels)
	sig.1 <- rev(sort(sig.levels))[1]
	final.quad.sig <- ifelse (P.I.vec <= sig.1, paste(class.vec, n.sig, sep=""), as.character("not sig."))
	k <- 2
	while (k <= n.sig)
	{
		sig.k <- rev(sort(sig.levels))[k]
		final.quad.sig[P.I.vec <= sig.k] <- paste(class.vec[P.I.vec <= sig.k], n.sig+1-k, sep="")
		k <- k+1
	}
  #	
	local.Moran <- data.frame(id=id, local.Moran=local.moran.vec, z.x=z.x, v.z.x=v.z.x.vec, E=E.I.vec, VAR=VAR.I.vec,
            SD=SD.I.vec, Z=Z.I.vec, P=P.I.vec, quad=class.vec, sig.df, final.quad=final.quad.sig)
	return(local.Moran)
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
	colnames(sig.df) <- paste("sig.quad.", sig.levels, sep="")

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

