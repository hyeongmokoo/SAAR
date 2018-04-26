# Program Purpose: 
#   Conducting bivariate SAS analyses.
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

global.lee.L <- function (x, y, nb, style="W", diag.zero=FALSE)
{
  # Purpose: Calculating global Lee's L
  # Arguments: 
  #   x: a vector of one variable
  #   y: a vector of another variable
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
  d.y <- y - mean(y)
  z.x <- d.x/(sqrt(sum(d.x^2)/n))
  z.y <- d.y/(sqrt(sum(d.y^2)/n))
  lx <- lag.listw(listw,d.x)
  ly <- lag.listw(listw,d.y)
  lee.l <- (n/nw)*sum(lx*ly)/(sqrt(sum((d.x)^2)) * sqrt(sum((d.y)^2)))
  return(lee.l)
}

global.pearson <- function(x, y, alternative="two.sided")
{
  # Purpose: Calculating Global Pearson's r
  # Arguments: 
  #   x: a vector of one variable
  #   y: a vector of another variable
  #   alternative: type of test

  alternative <- match.arg(alternative, c("greater", "less", "two.sided"))
  mf <- match.call(expand.dots = FALSE)

	n <- length(x)
	mu.x <- mean(x)	
	mu.y <- mean(y)
	sd.x <- sqrt(((n-1)/n)*var(x))
	sd.y <- sqrt(((n-1)/n)*var(y))
	z.x <- (x-mu.x)/sd.x
	z.y <- (y-mu.y)/sd.y	
	
	pearson.r <- sum(z.x*z.y)/n
  return(pearson.r)    
}

global.bMoran <- function (x, y, nb, style="W", diag.zero=TRUE)
{
  # Purpose: Calculating global Bivariate Moran
  # Arguments: 
  #   x: a vector of one variable
  #   y: a vector of another variable
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
  nw <- sum(rowSums(V))
  n <- length(x)
  d.x <- x - mean(x)
  d.y <- y - mean(y)
  z.x <- as.matrix(d.x/(sqrt(sum(d.x^2)/n)))
  z.y <- as.matrix(d.y/(sqrt(sum(d.y^2)/n)))
  lx <- lag.listw(listw,d.x)
  ly <- lag.listw(listw,d.y)
  warten.w <- (n/nw)*sum(d.x*ly)/(sqrt(sum((d.x)^2)) * sqrt(sum((d.y)^2)))
  return(warten.w)
}

global.bGeary <- function (x, y, nb, style="W", diag.zero=TRUE)
{
  # Purpose: ConduCalculating global Bivariate Geary
  # Arguments: 
  #   x: a vector of one variable
  #   y: a vector of another variable
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
  sn <- listw2sn(listw)

  V <- as(as(as_dgRMatrix_listw(listw), "RsparseMatrix"), "CsparseMatrix")
  nw <- sum(rowSums(V))
  n <- length(x)
  d.x <- x - mean(x)
  z.x <- d.x/(sqrt(sum(d.x^2)/n))
  d.y <- y - mean(y)
  z.y <- d.y/(sqrt(sum(d.y^2)/n))
  bGeary.C <- ((n-1)/(2*nw))*sum(sn[,3]*(x[sn[,1]]-x[sn[,2]])*(y[sn[,1]]-y[sn[,2]]))/(sqrt(sum((d.x)^2)) * sqrt(sum((d.y)^2)))
  return(bGeary.C)
}

local.lee.L <- function (x, y, id, nb, style="W", diag.zero=FALSE){
  # purpose: Calculating local Lee's Li 
  # Arguments:
  #   x: a vector of one variable
  #   y: a vector of another variable
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
  sum.VtV <- sum(row.sum^2)
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
	K <- n/sum.VtV
	local.lee.vec <- numeric(n)
      z.x.ij.sum.vec <- numeric(n)
	z.y.ij.sum.vec <- numeric(n)
	for (i in 1:n){
		vi.vec <- listw$weights[[i]]
		vi.nhbr.vec <- listw$neighbours[[i]]
		z.x.j <- z.x[vi.nhbr.vec]
		z.y.j <- z.y[vi.nhbr.vec]
		z.x.ij.sum <- sum(z.x.j*vi.vec)
		z.y.ij.sum <- sum(z.y.j*vi.vec)
		local.lee.l <- K*(z.x.ij.sum*z.y.ij.sum)
		z.x.ij.sum.vec[i] <- z.x.ij.sum
		z.y.ij.sum.vec[i] <- z.y.ij.sum
		local.lee.vec[i] <- local.lee.l
      }
	z.x.ij.sum.vec <- sqrt(K)*z.x.ij.sum.vec
	z.y.ij.sum.vec <- sqrt(K)*z.y.ij.sum.vec
	class.vec <- character(n)
	for (q in 1:n){
		if (z.x.ij.sum.vec[q] >= 0 & z.y.ij.sum.vec[q] >= 0) {
      	class.vec[q] <- "High-High"}
		else if (z.x.ij.sum.vec[q] < 0 & z.y.ij.sum.vec[q] >= 0) {
	  		class.vec[q] <- "Low-High"}
		else if (z.x.ij.sum.vec[q] < 0 & z.y.ij.sum.vec[q] < 0) {
			  class.vec[q] <- "Low-Low"}
  	else if (z.x.ij.sum.vec[q] >= 0 & z.y.ij.sum.vec[q] < 0) {
	  		class.vec[q] <- "High-Low"}
	}
 
     res.local.lee <- data.frame(id=id, local.L=local.lee.vec, v.z.x=z.x.ij.sum.vec, v.z.y=z.y.ij.sum.vec, quad=class.vec)
      return(res.local.lee)
}

local.pearson <- function (x, y, id){
  # purpose: Calculating local Pearson's ri
  # Arguments:
  #   x: a vector of one variable
  #   y: a vector of another variable
  #   id: a vector of ID codes

  n <- length(x)
  mu.x <- mean(x)
  mu.y <- mean(y)
  sd.x <- sqrt(sum((x - mu.x)^2)/n)
  sd.y <- sqrt(sum((y - mu.y)^2)/n)
  z.x <- (x-mu.x)/sd.x
  z.y <- (y-mu.y)/sd.y
  local.r.vec <- z.x*z.y
  class.vec <- character(n)
  for (q in 1:n){
	if (z.x[q] >= 0 & z.y[q] >= 0) {
     	class.vec[q] <- "High-High"}
	else if (z.x[q] < 0 & z.y[q] >= 0) {
  		class.vec[q] <- "Low-High"}
	else if (z.x[q] < 0 & z.y[q] < 0) {
            class.vec[q] <- "Low-Low"}
  	else if (z.x[q] >= 0 & z.y[q] < 0) {
  		class.vec[q] <- "High-Low"}
	}
  res.local.r <- data.frame(id=id, local.pearson=local.r.vec, z.x=z.x, z.y=z.y, quad=class.vec)
  return(res.local.r)
}

local.bMoran <- function (x, y, id, nb, style="W", diag.zero=FALSE){
  # purpose: Calculating local Bivariate Moran 
  #   x: a vector of one variable
  #   y: a vector of another variable
  #   nb: nb object
  #   style: style of spatial weight matrix (refer to nb2listw)
  #   diag.zero: if FALSE, each region has itself as its neighbour
  # Fuction called:
  #   nb.self

  style <- match.arg(style, c("B","C","S","W","U","V"))
 
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
	mu.x <- mean(x)
	mu.y <- mean(y)
	sd.x <- sqrt(sum((x - mu.x)^2)/n)
	sd.y <- sqrt(sum((y - mu.y)^2)/n)
	z.x <- (x-mu.x)/sd.x
	z.y <- (y-mu.y)/sd.y
	K <- n/sum.V
	local.bMoran.vec <- numeric(n)
	z.y.ij.sum.vec <- numeric(n)
	for (i in 1:n){
		vi.vec <- listw$weights[[i]]
		vi.nhbr.vec <- listw$neighbours[[i]]
            z.x.i <- z.x[i]
 		z.y.j <- z.y[vi.nhbr.vec]
		z.y.ij.sum <- sum(z.y.j*vi.vec)
		local.bMoran <- K*(z.x.i*z.y.ij.sum)
		z.y.ij.sum.vec[i] <- z.y.ij.sum
		local.bMoran.vec[i] <- local.bMoran
      }
	class.vec <- character(n)
	for (q in 1:n){
		if (z.x[q] >= 0 & z.y.ij.sum.vec[q] >= 0) {
      	class.vec[q] <- "High-High"}
		else if (z.x[q] < 0 & z.y.ij.sum.vec[q] >= 0) {
	  		class.vec[q] <- "Low-High"}
		else if (z.x[q] < 0 & z.y.ij.sum.vec[q] < 0) {
			  class.vec[q] <- "Low-Low"}
  	else if (z.x[q] >= 0 & z.y.ij.sum.vec[q] < 0) {
	  		class.vec[q] <- "High-Low"}
	}

      res.local.bMoran <- data.frame(id=id, local.bMoran=local.bMoran.vec, z.x=z.x, v.z.y=z.y.ij.sum.vec, quad=class.vec)
      return(res.local.bMoran)
}

local.bGeary <- function (x, y, id, nb, style="W", diag.zero=TRUE){
  # purpose: Calculating local Bivariate Geary 
  # Arguments:
  #   x: a vector of one variable
  #   y: a vector of another variable
  #   nb: nb object
  #   style: style of spatial weight matrix (refer to nb2listw)
  #   diag.zero: if FALSE, each region has itself as its neighbour
  # Fuction called:
  #   nb.self

  style <- match.arg(style, c("B","C","S","W","U","V"))
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

  	mu.x <- mean(x)
	sd.x <- sqrt(sum((x - mu.x)^2)/n)
	z.x <- (x-mu.x)/sd.x
  	mu.y <- mean(y)
	sd.y <- sqrt(sum((y - mu.y)^2)/n)
	z.y <- (y-mu.y)/sd.y
	K.origin <- (n-1)/(2*sum.V)
#	K <- n/sum.V
	local.bgeary.vec <- vector()
	for (i in 1:n){
		vi.vec <- listw$weights[[i]]
		vi.nhbr.vec <- listw$neighbours[[i]]
            z.x.i <- z.x[i]
		z.y.i <- z.y[i]
		z.x.j <- z.x[vi.nhbr.vec]
		z.y.j <- z.y[vi.nhbr.vec]
		local.bgeary.i <- K.origin*sum((z.x.i-z.x.j)*(z.y.i-z.y.j)*vi.vec)
		local.bgeary.vec[i] <- local.bgeary.i
      }
	class.vec <- vector()
	for (q in 1:n)
	{
		if (z.x[q] < 0 & z.y[q] < 0)
			class.vec[q] <- as.character("Low-Low")
		else if (z.x[q] < 0 & z.y[q] >= 0)
			class.vec[q] <- as.character("Low-High")
		else if (z.x[q] >= 0 & z.y[q] < 0)
			class.vec[q] <- as.character("High-Low")
		else if (z.x[q] >= 0 & z.y[q] >= 0)
			class.vec[q] <- as.character("High-High")
	}

      res.local.bgeary <- data.frame(id=id, local.sas=local.bgeary.vec, quad=class.vec)
      return(res.local.bgeary)
}

test.global.lee.L <- function (x, y, nb, style="W", alternative="two.sided", diag.zero=FALSE)
{
  # Purpose: Conducting significance testing for global Lee's L
  # Arguments: 
  #   x: a vector of one variable
  #   y: a vector of another variable
  #   nb: nb object
  #   style: style of spatial weight matrix (refer to nb2listw)
  #   alternative: type of test
  #   diag.zero: if FALSE, each region has itself as its neighbour
  # Fuction called:
  #   nb.self

  alternative <- match.arg(alternative, c("greater", "less", "two.sided"))
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
  d.y <- y - mean(y)
  z.x <- d.x/(sqrt(sum(d.x^2)/n))
  z.y <- d.y/(sqrt(sum(d.y^2)/n))
  lx <- lag.listw(listw,d.x)
  ly <- lag.listw(listw,d.y)
  lee.l <- (n/nw)*sum(lx*ly)/(sqrt(sum((d.x)^2)) * sqrt(sum((d.y)^2)))

  V <- crossprod(V)
  V <- V/sum(V)
  P <- (V + t(V))/2
  Y <- z.x%*%t(z.y)
  Q <- (Y + t(Y))/2

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

	E.L.off <- (F0.off*G0.off)/(n*(n-1))
	E.L.on <- (F0.on*G0.on)/n
	E.L <- E.L.off + E.L.on

	VAR.L.off <- ((2*F1.off*G1.off)/(n*(n-1)))+((4*(F2.off-F1.off)*(G2.off-G1.off))/(n*(n-1)*(n-2)))+ ((((F0.off^2)+(2*F1.off)-(4*F2.off))*((G0.off^2)+(2*G1.off)-(4*G2.off)))/(n*(n-1)*(n-2)*(n-3)))-(E.L.off^2)
	VAR.L.on <- (F1.on*G1.on/n)+(((F0.on^2-F1.on)*(G0.on^2-G1.on))/(n*(n-1)))-(E.L.on^2)
	COV.1 <- ((F2.all-F1.on-F2.off)*(G2.all-G1.on-G2.off))/(2*n*(n-1))
	COV.2 <- (((F0.on*F0.off)-(F2.all-F1.on-F2.off))*((G0.on*G0.off)-(G2.all-G1.on-G2.off)))/(n*(n-1)*(n-2))
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
  method <- "Global Lee's L test under randomization assumption"

  if (diag.zero==FALSE) style <- paste(style, "*",sep="")
  data.name <- paste(deparse(mf[[2]]), " & ",  deparse(mf[[3]]), "\nweights: ",  deparse(mf[[4]]), ", style: ", style, "\n", sep = "")
  res <- list(statistic = statistic, p.value = p.value, estimate = estimate,
        method = method, alternative = alternative, data.name = data.name)
  class(res) <- "htest"
  res
}

test.global.pearson <- function(x, y, alternative="two.sided")
{
  # Purpose: Conducting significance testing for global Pearson's r
  # Arguments: 
  #   x: a vector of one variable
  #   y: a vector of another variable
  #   alternative: type of test

  alternative <- match.arg(alternative, c("greater", "less", "two.sided"))
  mf <- match.call(expand.dots = FALSE)

	n <- length(x)
	mu.x <- mean(x)	
	mu.y <- mean(y)
	sd.x <- sqrt(((n-1)/n)*var(x))
	sd.y <- sqrt(((n-1)/n)*var(y))
	z.x <- (x-mu.x)/sd.x
	z.y <- (y-mu.y)/sd.y	
	
	pearson.r <- sum(z.x*z.y)/n
	
	E.r <- 0
	VAR.r <- 1/(n-1)
	SD.r <- sqrt(VAR.r)
	
	Z.r <- (pearson.r - E.r)/SD.r

  # Calculation p-values
	if (alternative=="two.sided")
		P.r <- 2 * pnorm(abs(Z.r), lower.tail = FALSE)
	else if (alternative=="greater")
		P.r <- pnorm(Z.r, lower.tail = FALSE)
	else P.r <- pnorm(Z.r)

  statistic <- Z.r
  attr(statistic, "names") <- "Pearson's r statistic standard deviate"
  p.value <- P.r
  estimate <- c(pearson.r, E.r, VAR.r)
  attr(estimate, "names") <- c("Observed Pearson's r", "Expectation","Variance")
  method <- "Global Pearson's r test under randomization assumption"

# if (diag.zero==FALSE) style <- paste(style, "*",sep="")
# data.name <- paste(deparse(mf[[2]]), " & ",  deparse(mf[[3]]), "\nweights: ",  deparse(mf[[4]]), ", style: ", style, "\n", sep = "")
  data.name <- paste(deparse(mf[[2]]), " & ",  deparse(mf[[3]]), sep = "")
  res <- list(statistic = statistic, p.value = p.value, estimate = estimate,
        method = method, alternative = alternative, data.name = data.name)
  class(res) <- "htest"
  res
}

test.global.bMoran <- function (x, y, nb, style="W", alternative="two.sided", diag.zero=TRUE)
{
  # Purpose: Conducting significance testing for global Bivariate Moran
  # Arguments: 
  #   x: a vector of one variable
  #   y: a vector of another variable
  #   nb: nb object
  #   style: style of spatial weight matrix (refer to nb2listw)
  #   alternative: type of test
  #   diag.zero: if FALSE, each region has itself as its neighbour
  # Fuction called:
  #   nb.self

  alternative <- match.arg(alternative, c("greater", "less", "two.sided"))
  style <- match.arg(style, c("B","C","S","W","U","V"))
  mf <- match.call(expand.dots = FALSE)

  # making non-zero diagonal elements in a spatial weight matrix
  if (style=="V") diag.zero <- FALSE
  if (diag.zero == FALSE) nb <- nb.self(nb)
  if (style=="V") {listw <- nb2listv(nb,beta=0.5)}
  else {listw <- nb2listw(nb,style=style)}

  V <- as(as(as_dgRMatrix_listw(listw), "RsparseMatrix"), "CsparseMatrix")
  nw <- sum(rowSums(V))
  n <- length(x)
  d.x <- x - mean(x)
  d.y <- y - mean(y)
  z.x <- as.matrix(d.x/(sqrt(sum(d.x^2)/n)))
  z.y <- as.matrix(d.y/(sqrt(sum(d.y^2)/n)))
  lx <- lag.listw(listw,d.x)
  ly <- lag.listw(listw,d.y)
  warten.w <- (n/nw)*sum(d.x*ly)/(sqrt(sum((d.x)^2)) * sqrt(sum((d.y)^2)))

	V <- V/sum(V)
	P <- 0.5*(V+t(V))
	Y <- z.x%*%t(z.y)
	Q <- 0.5*(Y+t(Y))

	P.off <- P
	diag(P.off) <- 0
	Q.off <- Q
	diag(Q.off) <- 0
	P.on <- diag(P)
	Q.on <- diag(Q)

	F0.off <- sum(P.off)
	F0.on <- sum(P.on)
	F1.off <- sum(P.off^2)
	F1.on <- sum(P.on^2)
	F2.off <- sum(rowSums(P.off)^2)
	F2.all <- sum(rowSums(P)^2)
	F0.all <- sum(P)
	F1.all <- sum(P^2)
	F2.on <- sum(P.on*rowSums(P))

	G0.off <- sum(Q.off)
	G0.on <- sum(Q.on)
	G1.off <- sum(Q.off^2)
	G1.on <- sum(Q.on^2)
	G2.off <- sum(rowSums(Q.off)^2)
	G2.all <- sum(rowSums(Q)^2)
	G0.all <- sum(Q)
	G1.all <- sum(Q^2)
	G2.on <- sum(Q.on*rowSums(Q))

	E.W.off <- (F0.off*G0.off)/(n*(n-1))
	E.W.on <- (F0.on*G0.on)/n
	E.W <- E.W.off + E.W.on

	VAR.W.off <- ((2*F1.off*G1.off)/(n*(n-1)))+((4*(F2.off-F1.off)*(G2.off-G1.off))/(n*(n-1)*(n-2)))+ ((((F0.off^2)+(2*F1.off)-(4*F2.off))*((G0.off^2)+(2*G1.off)-(4*G2.off)))/(n*(n-1)*(n-2)*(n-3)))-(E.W.off^2)
	VAR.W.on <- (F1.on*G1.on/n)+(((F0.on^2-F1.on)*(G0.on^2-G1.on))/(n*(n-1)))-(E.W.on^2)

	COV.1 <- ((F2.all-F1.on-F2.off)*(G2.all-G1.on-G2.off))/(2*n*(n-1))
	COV.2 <- (((F0.on*F0.off)-(F2.all-F1.on-F2.off))*((G0.on*G0.off)-(G2.all-G1.on-G2.off)))/(n*(n-1)*(n-2))
	COV.W <- (COV.1+COV.2)-(E.W.off*E.W.on)
	COV.W.2 <- 2*COV.W

	VAR.W <- VAR.W.off + VAR.W.on + COV.W.2

	SD.W <- sqrt(VAR.W)
	Z.W <- (warten.w - E.W)/SD.W

  # Calculation p-values
	if (alternative=="two.sided")
		P.W <- 2 * pnorm(abs(Z.W), lower.tail = FALSE)
	else if (alternative=="greater")
		P.W <- pnorm(Z.W, lower.tail = FALSE)
	else P.W <- pnorm(Z.W)

  statistic <- Z.W
  attr(statistic, "names") <- "Bivariate Moran standard deviate"
  p.value <- P.W
  estimate <- c(warten.w, E.W, VAR.W)
  attr(estimate, "names") <- c("Observed Bivariate Moran", "Expectation","Variance")
  method <- "Global Bivariate Moran test under randomization assumption"

  if (diag.zero==FALSE) style <- paste(style, "*",sep="")
  data.name <- paste(deparse(mf[[2]]), " & ",  deparse(mf[[3]]), "\nweights: ",  deparse(mf[[4]]), ", style: ", style, "\n", sep = "")
  res <- list(statistic = statistic, p.value = p.value, estimate = estimate,
        method = method, alternative = alternative, data.name = data.name)
  class(res) <- "htest"
  res
}

test.global.bGeary <- function (x, y, nb, style="W", alternative="two.sided", diag.zero=TRUE)
{
  # Purpose: Conducting significance testing for global Bivariate Geary
  # Arguments: 
  #   x: a vector of one variable
  #   y: a vector of another variable
  #   nb: nb object
  #   style: style of spatial weight matrix (refer to nb2listw)
  #   alternative: type of test
  #   diag.zero: if FALSE, each region has itself as its neighbour
  # Fuction called:
  #   nb.self

  alternative <- match.arg(alternative, c("greater", "less", "two.sided"))
  style <- match.arg(style, c("B","C","S","W","U","V"))
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
  d.y <- y - mean(y)
  z.y <- d.y/(sqrt(sum(d.y^2)/n))
  bGeary.C <- ((n-1)/(2*nw))*sum(sn[,3]*(x[sn[,1]]-x[sn[,2]])*(y[sn[,1]]-y[sn[,2]]))/(sqrt(sum((d.x)^2)) * sqrt(sum((d.y)^2)))

	XY <- z.x%*%t(z.y)
	sum.V <- sum(V)
	sV <- 0.5*(V+t(V))
	row.sum <- rowSums(sV)
	Omega.matrix <- V*0
	diag(Omega.matrix) <- row.sum
	Omega.sV <- (Omega.matrix - sV)
	Omega.sV <- ((n-1)/n)*(Omega.sV/sum.V)
#	geary.c <- sum(Omega.sV*XY)
	P <- Omega.sV
	Q <- 0.5*(XY+t(XY))
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
	Z.C <- (bGeary.C - E.C)/SD.C

  # Calculation p-values
	if (alternative=="two.sided")
		P.C <- 2 * pnorm(abs(Z.C), lower.tail = FALSE)
	else if (alternative=="greater")
		P.C <- pnorm(Z.C, lower.tail = FALSE)
	else P.C <- pnorm(Z.C)

  statistic <- Z.C
  attr(statistic, "names") <- "Bivariate Geary statistic standard deviate"
  p.value <- P.C
  estimate <- c(bGeary.C, E.C, VAR.C)
  attr(estimate, "names") <- c("Observed Bivariate Geary", "Expectation","Variance")
  method <- "Global Bivariate Geary test under randomization assumption"

  if (diag.zero==FALSE) style <- paste(style, "*",sep="")
  data.name <- paste(deparse(mf[[2]]), " & ",  deparse(mf[[3]]), "\nweights: ",  deparse(mf[[4]]), ", style: ", style, "\n", sep = "")
  res <- list(statistic = statistic, p.value = p.value, estimate = estimate,
        method = method, alternative = alternative, data.name = data.name)
  class(res) <- "htest"
  res
}

test.local.lee.L <- function (x, y, id, nb, style="W", sig.levels=c(0.05), method="total", alternative="two.sided", diag.zero=FALSE){
  # purpose: Calculating local Lee's Li and conducting significance testing
  # Arguments:
  #   x: a vector of one variable
  #   y: a vector of another variable
  #   id: a vector of ID codes
  #   nb: nb object
  #   sig.levels: a vector of significance levels	
  #   style: style of spatial weight matrix (refer to nb2listw)
  #   method: "conditional" or "total" randomization assumption for null hypothesis
  #   alternatie: type of test
  #   diag.zero: if FALSE, each region has itself as its neighbour
  # Fuction called:
  #   nb.self

  alternative <- match.arg(alternative, c("greater", "less", "two.sided"))
  style <- match.arg(style, c("B","C","S","W","U","V"))
  method <- match.arg(method, c("conditional", "total"))
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
		else if (method=="total") {
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
#	  VAR.L.on.vec[i] <- VAR.L.on
#	  VAR.L.off.vec[i] <- VAR.L.off
#	  COV.L.2.vec[i] <- COV.L.2
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
	z.x.ij.sum.vec <- sqrt(K)*z.x.ij.sum.vec
	z.y.ij.sum.vec <- sqrt(K)*z.y.ij.sum.vec
	class.vec <- character(n)
	for (q in 1:n){
		if (z.x.ij.sum.vec[q] >= 0 & z.y.ij.sum.vec[q] >= 0) {
      	class.vec[q] <- "High-High"}
		else if (z.x.ij.sum.vec[q] < 0 & z.y.ij.sum.vec[q] >= 0) {
	  		class.vec[q] <- "Low-High"}
		else if (z.x.ij.sum.vec[q] < 0 & z.y.ij.sum.vec[q] < 0) {
			  class.vec[q] <- "Low-Low"}
  	else if (z.x.ij.sum.vec[q] >= 0 & z.y.ij.sum.vec[q] < 0) {
	  		class.vec[q] <- "High-Low"}
	}
 
  # Significance for different levels
	sig.df <- as.data.frame(sapply(sig.levels, function(x, p.l, c.d) {ifelse(p.l > x, "not sig.", c.d)}, p.l=P.L.vec, c.d=class.vec))
	colnames(sig.df) <- paste("sig.quad.", sig.levels, sep="")

  # To make a composite vector of quad and the order of significance
	n.sig <- length(sig.levels)
	sig.1 <- rev(sort(sig.levels))[1]
	final.quad.sig <- ifelse (P.L.vec <= sig.1, paste(class.vec, n.sig, sep=""), as.character("not sig."))
	k <- 2
	while (k <= n.sig)
	{
		sig.k <- rev(sort(sig.levels))[k]
		final.quad.sig[P.L.vec <= sig.k] <- paste(class.vec[P.L.vec <= sig.k], n.sig+1-k, sep="")
		k <- k+1
	}
  #	
	local.lee <- data.frame(id=id, local.L=local.lee.vec, z.x=z.x, z.y=z.y, 
		v.z.x=z.x.ij.sum.vec, v.z.y=z.y.ij.sum.vec, E=E.L.vec, VAR=VAR.L.vec,
            SD=SD.L.vec, Z=Z.L.vec, P=P.L.vec, quad=class.vec, sig.df, final.quad=final.quad.sig)
	return(local.lee)
}

test.local.pearson <- function (x, y, id, sig.levels=c(0.05), method="randomization", alternative="two.sided"){
  # purpose: Calculating local Pearson's ri and conducting significance testing
  # Arguments:
  #   x: a vector of one variable
  #   y: a vector of another variable
  #   id: a vector of ID codes
  #   sig.levels: a vector of significance levels	
  #   method: "randomization" or "normality" assumption for null hypothesis
  #   alternatie: type of test

  alternative <- match.arg(alternative, c("greater", "less", "two.sided"))
#  mf <- match.call(expand.dots = FALSE)

  n <- length(x)

	mu.x <- mean(x)
	mu.y <- mean(y)
	sd.x <- sqrt(sum((x - mu.x)^2)/n)
	sd.y <- sqrt(sum((y - mu.y)^2)/n)
	z.x <- (x-mu.x)/sd.x
	z.y <- (y-mu.y)/sd.y

  # calculating a local R value

	if (method=="normality")
	{
	local.r.vec <- z.x*z.y
	e.r <- cor(x, y)
	E.R.vec <- rep(e.r, n)
	var.r <- 1+cor(x, y)^2
	VAR.R.vec <- rep(var.r, n)
	SD.R.vec <- sqrt(VAR.R.vec)
	skew.r <- (2*cor(x, y)*(3+cor(x, y)^2))/(1+cor(x, y)^2)^(3/2)
	Z.R.vec <- (local.r.vec - E.R.vec)/SD.R.vec
	}

	else if (method=="randomization")
	{
	local.r.vec <- z.x*z.y
	e.r <- cor(x, y)
	E.R.vec <- rep(e.r, n)
	var.r <- mean(z.x^2*z.y^2)-cor(x, y)^2
	VAR.R.vec <- rep(var.r, n)
	SD.R.vec <- sqrt(VAR.R.vec)
	Z.R.vec <- (local.r.vec - E.R.vec)/SD.R.vec
	}
	
  # calculating p-values
	#P.L.vec <- numeric(n)
	if (alternative=="two.sided") {
	    P.R.vec <- sapply(Z.R.vec, function(x) {2 * pnorm(abs(x), lower.tail = FALSE)})}
	else if (alternative=="greater") {
   		P.R.vec <- sapply(Z.R.vec, function(x) {pnorm(x, lower.tail = FALSE)})}
	else {
      P.R.vec <- sapply(Z.R.vec, function(x) {pnorm(x, lower.tail = TRUE)})}

  # an analogy of Moran scatterplot
	class.vec <- character(n)
	for (q in 1:n){
		if (z.x[q] >= 0 & z.y[q] >= 0) {
      	class.vec[q] <- "High-High"}
		else if (z.x[q] < 0 & z.y[q] >= 0) {
	  		class.vec[q] <- "Low-High"}
		else if (z.x[q] < 0 & z.y[q] < 0) {
			  class.vec[q] <- "Low-Low"}
  	else if (z.x[q] >= 0 & z.y[q] < 0) {
	  		class.vec[q] <- "High-Low"}
	}
 
  # Significance for different levels
	sig.df <- as.data.frame(sapply(sig.levels, function(x, p.l, c.d) {ifelse(p.l > x, "not sig.", c.d)}, p.l=P.R.vec, c.d=class.vec))
	colnames(sig.df) <- paste("sig.quad.", sig.levels, sep="")

  # To make a composite vector of quad and the order of significance
	n.sig <- length(sig.levels)
	sig.1 <- rev(sort(sig.levels))[1]
	final.quad.sig <- ifelse (P.R.vec <= sig.1, paste(class.vec, n.sig, sep=""), as.character("not sig."))
	k <- 2
	while (k <= n.sig)
	{
		sig.k <- rev(sort(sig.levels))[k]
		final.quad.sig[P.R.vec <= sig.k] <- paste(class.vec[P.R.vec <= sig.k], n.sig+1-k, sep="")
		k <- k+1
	}
  #	

	local.pearson <- data.frame(id=id, pearson.r=local.r.vec, z.x=z.x, z.y=z.y, E=E.R.vec, VAR=VAR.R.vec,
                 SD=SD.R.vec, Z=Z.R.vec, P=P.R.vec, quad=class.vec, sig.df, pearson.final.quad=final.quad.sig)

	return(local.pearson)

}

test.local.bMoran <- function (x, y, id, nb, style="W", sig.levels=c(0.05), method="total", alternative="two.sided", diag.zero=FALSE){
  # purpose: Calculating local Bivariate Moran and conducting significance testing
  # Arguments:
  #   x: a vector of one variable
  #   y: a vector of another variable
  #   id: a vector of ID codes
  #   nb: nb object
  #   sig.levels: a vector of significance levels	
  #   style: style of spatial weight matrix (refer to nb2listw)
  #   method: "conditional" or "total" randomization assumption for null hypothesis
  #   alternatie: type of test
  #   diag.zero: if FALSE, each region has itself as its neighbour
  # Fuction called:
  #   nb.self

  alternative <- match.arg(alternative, c("greater", "less", "two.sided"))
  style <- match.arg(style, c("B","C","S","W","U","V"))
  method <- match.arg(method, c("conditional", "total"))
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

	mu.x <- mean(x)
	mu.y <- mean(y)
	sd.x <- sqrt(sum((x - mu.x)^2)/n)
	sd.y <- sqrt(sum((y - mu.y)^2)/n)
	z.x <- (x-mu.x)/sd.x
	z.y <- (y-mu.y)/sd.y
	z.x.2 <- z.x^2
	z.y.2 <- z.y^2
	z.xy.2 <- z.x.2*z.y.2

	K <- n/sum.V

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
	local.bMoran.vec <- numeric(n)
	E.L.vec <- numeric(n)
	E.L.on.vec <- numeric(n)
	E.L.off.vec <- numeric(n)
	VAR.L.vec <- numeric(n)
	z.y.ij.sum.vec <- numeric(n)

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
            z.y.i <- z.y[i]
		z.y.j <- z.y[vi.nhbr.vec]
		z.y.ij.sum <- sum(z.y.j*vi.vec)
		z.y.ij.sum.vec[i] <- z.y.ij.sum

  # calculating a local L value
		local.bMoran <- K*(z.x.i*z.y.ij.sum)
		local.bMoran.vec[i] <- local.bMoran

  # calculating an expectation and variance
		if (method=="conditional") {
		E.L.off <- (-1*K/(n-1))*(vi.s-vii)*(z.x.i*z.y.i)
		E.L.on <- K*vii*(z.x.i*z.y.i)
		E.L <- E.L.off + E.L.on
		VAR.L <- (K^2)*(n/((n-2)*((n-1)^2)))*((n-1)*(vi.2.s-(vii^2))-((vi.s-vii)^2))*(z.x.i^2)*(n-1-(z.y.i^2))

		}
		else {
		E.L.off <- -1*K*(vi.s-vii)*r.xy/(n-1)
		E.L.on <- K*vii*r.xy
		E.L <- E.L.off + E.L.on
		VAR.L.off <- ((K^2)*(((vi.2.s-vii.2)*(n-(b1.xy*r.xy.2))/(n-1))+((((vi.2.s-vii.2)-((vi.s-vii)^2))*(n-(2*b1.xy*r.xy.2)))/((n-1)*(n-2)))))-(E.L.off^2)
		VAR.L.on <- (K^2)*r.xy.2*(b1.xy-1)*vii.2
		COV.L <- -1*(K^2)*vii*(vi.s-vii)*r.xy.2*(b1.xy-1)/(n-1)
	      COV.L.2 <- 2*COV.L
            VAR.L <- VAR.L.off + VAR.L.on + COV.L.2

    }

    # store values
	  E.L.vec[i] <- E.L
	  E.L.on.vec[i] <- E.L.on
	  E.L.off.vec[i] <- E.L.off
	  VAR.L.vec[i] <- VAR.L
	}

	SD.L.vec <- sqrt(VAR.L.vec)
	Z.L.vec <- (local.bMoran.vec-E.L.vec)/SD.L.vec

  # calculating p-values
	#P.L.vec <- numeric(n)
	if (alternative=="two.sided") {
	    P.L.vec <- sapply(Z.L.vec, function(x) {2 * pnorm(abs(x), lower.tail = FALSE)})}
	else if (alternative=="greater") {
   		P.L.vec <- sapply(Z.L.vec, function(x) {pnorm(x, lower.tail = FALSE)})}
	else {
      P.L.vec <- sapply(Z.L.vec, function(x) {pnorm(x, lower.tail = TRUE)})}

  # an analogy of Moran scatterplot
	class.vec <- character(n)
	for (q in 1:n){
		if (z.x[q] >= 0 & z.y.ij.sum.vec[q] >= 0) {
      	class.vec[q] <- "High-High"}
		else if (z.x[q] < 0 & z.y.ij.sum.vec[q] >= 0) {
	  		class.vec[q] <- "Low-High"}
		else if (z.x[q] < 0 & z.y.ij.sum.vec[q] < 0) {
			  class.vec[q] <- "Low-Low"}
  	else if (z.x[q] >= 0 & z.y.ij.sum.vec[q] < 0) {
	  		class.vec[q] <- "High-Low"}
	}
 
  # Significance for different levels
	sig.df <- as.data.frame(sapply(sig.levels, function(x, p.l, c.d) {ifelse(p.l > x, "not sig.", c.d)}, p.l=P.L.vec, c.d=class.vec))
	colnames(sig.df) <- paste("sig.quad.", sig.levels, sep="")

  # To make a composite vector of quad and the order of significance
	n.sig <- length(sig.levels)
	sig.1 <- rev(sort(sig.levels))[1]
	final.quad.sig <- ifelse (P.L.vec <= sig.1, paste(class.vec, n.sig, sep=""), as.character("not sig."))
	k <- 2
	while (k <= n.sig)
	{
		sig.k <- rev(sort(sig.levels))[k]
		final.quad.sig[P.L.vec <= sig.k] <- paste(class.vec[P.L.vec <= sig.k], n.sig+1-k, sep="")
		k <- k+1
	}
  #	
	local.bMoran <- data.frame(id=id, local.bMoran=local.bMoran.vec, z.x=z.x, z.y=z.y, 
		v.z.y=z.y.ij.sum.vec, E=E.L.vec, VAR=VAR.L.vec,
            SD=SD.L.vec, Z=Z.L.vec, P=P.L.vec, quad=class.vec, sig.df, final.quad=final.quad.sig)
	return(local.bMoran)

}

test.local.bGeary <- function (x, y, id, nb, style="W", sig.levels=c(0.05), method="total", alternative="two.sided", diag.zero=TRUE){
  # purpose: Calculating local Bivariate Geary and conducting significance testing
  # Arguments:
  #   x: a vector of one variable
  #   y: a vector of another variable
  #   id: a vector of ID codes
  #   nb: nb object
  #   sig.levels: a vector of significance levels	
  #   style: style of spatial weight matrix (refer to nb2listw)
  #   method: "conditional" or "total" randomization assumption for null hypothesis
  #   alternatie: type of test
  #   diag.zero: if FALSE, each region has itself as its neighbour
  # Fuction called:
  #   nb.self

  alternative <- match.arg(alternative, c("greater", "less", "two.sided"))
  style <- match.arg(style, c("B","C","S","W","U","V"))
  method <- match.arg(method, c("conditional", "total"))
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

  # some basic calculations
  	mu.x <- mean(x)
	sd.x <- sqrt(sum((x - mu.x)^2)/n)
	z.x <- (x-mu.x)/sd.x
	z.x.2 <- z.x^2
  	mu.y <- mean(y)
	sd.y <- sqrt(sum((y - mu.y)^2)/n)
	z.y <- (y-mu.y)/sd.y
	z.y.2 <- z.y^2
	z.x.2.z.y.2 <- z.x.2+z.y.2
	z.xy.2 <- z.x.2*z.y.2

	K.origin <- (n-1)/(2*sum.V)
	K <- n/sum.V

	m2.x <- sum((x-mean(x))^2)/n
	m2.y <- sum((y-mean(y))^2)/n
	m1.xy <- sum((x-mean(x))*(y-mean(y))) / n
	m2.xy <- sum( ((x-mean(x))^2) * ((y-mean(y))^2) ) / n
	m12.xy <- sum( (x-mean(x)) * ((y-mean(y))^2) ) / n
	m21.xy <- sum( ((x-mean(x))^2) * (y-mean(y)) ) / n
	b1.xy <- m2.xy/m1.xy^2
	b12.xy <- m12.xy/(sqrt(m2.y)*m1.xy)
	b21.xy <- m21.xy/(sqrt(m2.x)*m1.xy)
	r.xy <- cor(x,y)
	r.xy.2 <- r.xy^2
	b2.s <- b1.xy*r.xy.2

  # objects to store results

	local.bgeary.vec <- vector()
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
		z.y.i <- z.y[i]
		z.x.i.2 <- z.x.i^2
		z.y.i.2 <- z.y.i^2
		z.x.j <- z.x[vi.nhbr.vec]
		z.y.j <- z.y[vi.nhbr.vec]
#		v.z.x.i <- sum(z.x.j*nhbr.i[,3])
#		v.z.y.i <- sum(z.y.j*nhbr.i[,3])
#		v.z.x.vec[i] <- v.z.x.i
#		v.z.y.vec[i] <- v.z.y.i
  # calculating a local C value
		local.bgeary.i <- K.origin*sum((z.x.i-z.x.j)*(z.y.i-z.y.j)*vi.vec)
		local.bgeary.vec[i] <- local.bgeary.i
  # calculating an expectation and variance
		if (method=="conditional") {
		E.C <- 0.5*K*(vi.s-vii)*(z.x.i*z.y.i+r.xy)
		VAR.C <- (((K/(2*n))^2/(n-2))*(((n-1)*(vi.2.s-vii.2))-(vi.s-vii)^2))*
		(((n-1)*(((b1.xy*r.xy-(2*z.x.i*b12.xy)
		-(2*z.y.i*b21.xy)+(2*z.x.i*z.y.i))*n*r.xy)+((z.x.i.2+z.y.i.2)*n)-(z.x.i.2*z.y.i.2)))
		-(n*r.xy+(z.x.i*z.y.i))^2)
		}
		else if (method=="total") {
		E.C.off <- K*(vi.s-vii)*r.xy/n
		E.C.on <- K*((n-1)/n)*(vi.s-vii)*r.xy
		E.C <- E.C.off + E.C.on
		VAR.C <- (((K^2)*(n-1))/(4*(n^2)))*((2*n*(((vi.2.s-vii.2)-(vi.s-vii)^2)*(1-r.xy.2))/(n-2))
		+(((-1)*(2*(vi.s-vii)*n*r.xy)^2)/(n-1))
		+((((vi.2.s-vii.2)+(3*((vi.s-vii)^2)))*n*r.xy.2)+(((vi.2.s-vii.2)+(vi.s-vii)^2)*n*b1.xy*r.xy.2)+(2*n*(vi.2.s-vii.2))))
		}

    # store values
	  E.C.vec[i] <- E.C
	  VAR.C.vec[i] <- VAR.C
	}

	SD.C.vec <- sqrt(VAR.C.vec)
	Z.C.vec <- (local.bgeary.vec-E.C.vec)/SD.C.vec

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
		if (z.x[q] < 0 & z.y[q] < 0)
			class.vec[q] <- as.character("Low-Low")
		else if (z.x[q] < 0 & z.y[q] >= 0)
			class.vec[q] <- as.character("Low-High")
		else if (z.x[q] >= 0 & z.y[q] < 0)
			class.vec[q] <- as.character("High-Low")
		else if (z.x[q] >= 0 & z.y[q] >= 0)
			class.vec[q] <- as.character("High-High")
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
	local.bGeary <- data.frame(id=id, local.bGeary=local.bgeary.vec, z.x=z.x, z.y=z.y, E=E.C.vec, VAR=VAR.C.vec,
            SD=SD.C.vec, Z=Z.C.vec, P=P.C.vec, quad=class.vec, sig.df, final.quad=final.quad.sig)
	return(local.bGeary)
}


