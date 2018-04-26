scatterplot.local.moran <- function (x, nb, style="W", diag.zero=TRUE)
{
  # Purpose: Drawing Moran scatterplots
  # Arguments: 
  #   x: a vector of one variable
  #   y: a vector of another variable
  #   nb: nb object
  #   style: style of spatial weight matrix (refer to nb2listw)
  #   alternative: type of test
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


	n <- length(x)
	K <- n/sum(unlist(lapply(listw$weights,sum)))

	mean.x <- mean(x)
	sd.x <- sqrt(sum((x - mean.x)^2)/n)
	z.x <- (x - mean.x)/sd.x
	v.z.x <- K*lag.listw(listw,z.x)

	plot(z.x, v.z.x, main="Moran Scatterplot", xlab="z-transformed X", ylab="z-transformed SL of X", lwd=1)
	abline(lm(v.z.x~z.x), lty=1)
	abline(h=0, v=0, lty=8, lwd=1)
	invisible ()
}

scatterplot.local.lee <- function (x, y, nb, style="W", diag.zero=FALSE)
{
  # Purpose: Drawing Lee's L scatterplots
  # Arguments: 
  #   x: a vector of one variable
  #   y: a vector of another variable
  #   nb: nb object
  #   style: style of spatial weight matrix (refer to nb2listw)
  #   alternative: type of test
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

	n <- length(x)

	row.sum <- unlist(lapply(listw$weights,sum))
	sum.VtV <- sum(row.sum^2)
	K <- n/sum.VtV
	sqrt.K <- sqrt(K)

	mu.x <- mean(x)
	sd.x <- sqrt(sum((x-mu.x)^2)/n)
	mu.y <- mean(y)
	sd.y <- sqrt(sum((y-mu.y)^2)/n)
	z.x <- (x-mu.x)/sd.x
	z.y <- (y-mu.y)/sd.y
	v.z.x <- sqrt.K*lag.listw(listw,z.x)
	v.z.y <- sqrt.K*lag.listw(listw,z.y)
	plot(v.z.x, v.z.y, main="Lee's L Scatterplot", xlab="z-transformed SMA of X", ylab="z-transformed SMA of Y", lwd=1)
	abline(lm(v.z.y~v.z.x), lty=1)
	abline(h=0, v=0, lty=8, lwd=1)
	invisible()
}

scatterplot.local.pearson <- function (x, y)
{
  # Purpose: Drawing Pearson scatterplots
  # Arguments: 
  #   x: a vector of one variable
  #   y: a vector of another variable

	n <- length(x)
	sd.x <- sqrt(sum((x - mean(x))^2)/n)
	z.x <- (x - mean(x))/sd.x
	sd.y <- sqrt(sum((y - mean(y))^2)/n)
	z.y <- (y - mean(y))/sd.y
	plot(z.x, z.y, main="Local Pearson Scatterplot", xlab="z-transformed X", ylab="z-transformed Y", lwd=1)
	abline(lm(z.y~z.x), lty=1)
	abline(h=0, v=0, lty=8, lwd=1)
	invisible()
}

scatterplot.local.bMoran <- function (x, y, nb, style="W", diag.zero=TRUE)
{
  # Purpose: Drawing Bivaraite Moran scatterplots
  # Arguments: 
  #   x: a vector of one variable
  #   y: a vector of another variable
  #   nb: nb object
  #   style: style of spatial weight matrix (refer to nb2listw)
  #   alternative: type of test
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

	n <- length(x)
	K <- n/sum(unlist(lapply(listw$weights,sum)))

	n <- length(x)
	sd.x <- sqrt(sum((x - mean(x))^2)/n)
	z.x <- (x - mean(x))/sd.x
	sd.y <- sqrt(sum((y - mean(y))^2)/n)
	z.y <- (y - mean(y))/sd.y
	v.z.y <- K*lag.listw(listw,z.y)

	plot(z.x, v.z.y, main="Bivariate Moran Scatterplot", xlab="z-transformed X", ylab="z-transformed SL of Y", lwd=1)
	abline(lm(v.z.y~z.x), lty=1)
	abline(h=0, v=0, lty=8, lwd=1)
	invisible ()
}

spatial.co.clustogram <- function (x, y, nb, maxsr, type.higher.nb="inclusive", type.row.stand="regular", alternative="two", diag.zero=FALSE)
{
  # Purpose: Drawing spatial co-clustograms using Lee's L*
  # Arguments: 
  #   x: a vector of one variable
  #   y: a vector of another variable
  #   nb: nb object
  #   style <- match.arg(style, c("B","C","S","W","U","V"))
  #   maxsr: the maximum number of spatial ranges
  #   type.higher.nb: types of making higher-order nb objects 	
  #   type.row.stand: types of doing row-standardization of SPM 	
  #   alternative: type of test
  #   diag.zero: if FALSE, each region has itself as its neighbour
  # Fuction called:
  #   nb.self
  #   nblag.generator
  #   test.global.pearson
  #   test.global.lee.L

  alternative <- match.arg(alternative, c("greater", "less", "two.sided"))
# style <- match.arg(style, c("B","C","S","W","U"))
  type.higher.nb <- match.arg(type.higher.nb, c("inclusive","exclusive"))
  type.row.stand <- match.arg(type.row.stand, c("regular","special", "none"))
  mf <- match.call(expand.dots = FALSE)

	n <- length(x)
	maxsr.sg <- round(sqrt(n)/2) 
	if (maxsr > maxsr.sg) maxsr <- maxsr.sg

	global.pearson.p <- test.global.pearson(x, y, alternative=alternative)$statistic
	global.sas.p.vec <- as.vector(global.pearson.p)
	
	if (type.higher.nb=="inclusive") {
	nblag.list <- nblag.generator(nb, maxlag=maxsr, type="inclusive")
      }
	else if (type.higher.nb=="exclusive") {
	nblag.list <- nblag.generator(nb, maxlag=maxsr, type="exclusive")
      }

	k <- maxsr
	for (i in 1:k)
	{
		nblag.i <- nblag.list[[i]]
		if (type.row.stand=="special")
		{
		global.sas.p.i <- test.global.lee.L(x, y, nblag.i, style="V", alternative=alternative, diag.zero=diag.zero)$statistic
		}
		else if (type.row.stand=="regular")
		{
		global.sas.p.i <- test.global.lee.L(x, y, nblag.i, style="W", alternative=alternative, diag.zero=diag.zero)$statistic
		}
		else if (type.row.stand=="none")
		{
		global.sas.p.i <- test.global.lee.L(x, y, nblag.i, style="B", alternative=alternative, diag.zero=diag.zero)$statistic
		}
		global.sas.p.vec <- cbind(global.sas.p.vec, global.sas.p.i)
	}

	plot(0:k, global.sas.p.vec, type="b", main="Spatial Co-clustogram", xlab="Spatial Ranges", ylab="Lee's L* (Z-values)", lwd=1)	
	axis(1,0:k)
#	plot(1:k, global.sas.p.vec[-1], type="b", xlab="Spatial Ranges", ylab="Lee's L* (Z-values)", lwd=1)	
	abline(h=2, lty=8, lwd=1)
	abline(h=-2, lty=8, lwd=1)
	invisible()
}

spatial.clustogram <- function (x, nb, maxsr, type.higher.nb="inclusive", type.row.stand="regular", alternative="two", diag.zero=FALSE)
{
  # Purpose: Drawing spatial clustograms using Lee's S*
  # Arguments: 
  #   x: a vector of one variable
  #   nb: nb object
  #   style <- match.arg(style, c("B","C","S","W","U"))
  #   maxsr: the maximum number of spatial ranges
  #   type.higher.nb: types of making higher-order nb objects 	
  #   type.row.stand: types of doing row-standardization of SPM 	
  #   alternative: type of test
  #   diag.zero: if FALSE, each region has itself as its neighbour
  # Fuction called:
  #   nb.self
  #   nblag.generator
  #   test.global.pearson
  #   test.global.lee.S

  alternative <- match.arg(alternative, c("greater", "less", "two.sided"))
# style <- match.arg(style, c("B","C","S","W","U"))
  type.higher.nb <- match.arg(type.higher.nb, c("inclusive","exclusive"))
  type.row.stand <- match.arg(type.row.stand, c("regular","special", "none"))
  mf <- match.call(expand.dots = FALSE)

	n <- length(x)
	maxsr.sg <- round(sqrt(n)/2) 
	if (maxsr > maxsr.sg) maxsr <- maxsr.sg

	global.z2.p <- sqrt(n-1)
      global.sas.p.vec <- as.vector(global.z2.p)
	
	if (type.higher.nb=="inclusive") {
	nblag.list <- nblag.generator(nb, maxlag=maxsr, type="inclusive")
      }
	else if (type.higher.nb=="exclusive") {
	nblag.list <- nblag.generator(nb, maxlag=maxsr, type="exclusive")
      }

	k <- maxsr
	for (i in 1:k)
	{
		nblag.i <- nblag.list[[i]]
		if (type.row.stand=="special")
		{
		global.sas.p.i <- test.global.lee.S(x, nblag.i, style="V", method="randomization", alternative=alternative, diag.zero=diag.zero)$statistic
		}
		else if (type.row.stand=="regular")
		{
		global.sas.p.i <- test.global.lee.S(x, nblag.i, style="W", method="randomization", alternative=alternative, diag.zero=diag.zero)$statistic
		}
		else if (type.row.stand=="none")
		{
		global.sas.p.i <- test.global.lee.S(x, nblag.i, style="B", method="randomization", alternative=alternative, diag.zero=diag.zero)$statistic
		}
		global.sas.p.vec <- cbind(global.sas.p.vec, global.sas.p.i)
	}

	plot(0:k, global.sas.p.vec, type="b", main="Spatial Clustogram", xlab="Spatial Ranges", ylab="Lee's S* (Z-values)", lwd=1)	
	axis(1,0:k)
#	plot(1:k, global.sas.p.vec[-1], type="b", xlab="Spatial Ranges", ylab="Lee's S* (Z-values)", lwd=1)	
	abline(h=2, lty=8, lwd=1)
	abline(h=-2, lty=8, lwd=1)
	invisible()
}

spatial.correlogram <- function (x, nb, maxsr, type.higher.nb="exclusive", type.row.stand="regular", alternative="two", diag.zero=TRUE)
{
  # Purpose: Drawing spatial correlograms
  # Arguments: 
  #   x: a vector of one variable
  #   nb: nb object
  #   style <- match.arg(style, c("B","C","S","W","U"))
  #   maxsr: the maximum number of spatial ranges
  #   type.higher.nb: types of making higher-order nb objects 	
  #   type.row.stand: types of doing row-standardization of SPM 	
  #   alternative: type of test
  #   diag.zero: if FALSE, each region has itself as its neighbour
  # Fuction called:
  #   nb.self
  #   nblag.generator
  #   test.global.pearson
  #   test.global.moran

  alternative <- match.arg(alternative, c("greater", "less", "two.sided"))
# style <- match.arg(style, c("B","C","S","W","U"))
  type.higher.nb <- match.arg(type.higher.nb, c("inclusive","exclusive"))
  type.row.stand <- match.arg(type.row.stand, c("regular","special", "none"))
  mf <- match.call(expand.dots = FALSE)

	n <- length(x)
	maxsr.sg <- round(sqrt(n)/2) 
	if (maxsr > maxsr.sg) maxsr <- maxsr.sg

	global.pearson.p <- test.global.pearson(x, x, alternative=alternative)$statistic
	global.sas.p.vec <- as.vector(global.pearson.p)
	
	if (type.higher.nb=="inclusive") {
	nblag.list <- nblag.generator(nb, maxlag=maxsr, type="inclusive")
      }
	else if (type.higher.nb=="exclusive") {
	nblag.list <- nblag.generator(nb, maxlag=maxsr, type="exclusive")
      }

	k <- maxsr
	for (i in 1:k)
	{
		nblag.i <- nblag.list[[i]]
		if (type.row.stand=="special")
		{
		global.sas.p.i <- test.global.moran(x, nblag.i, style="V", method="randomization", alternative=alternative, diag.zero=diag.zero)$statistic
		}
		else if (type.row.stand=="regular")
		{
		global.sas.p.i <- test.global.moran(x, nblag.i, style="W", method="randomization", alternative=alternative, diag.zero=diag.zero)$statistic
		}
		else if (type.row.stand=="none")
		{
		global.sas.p.i <- test.global.moran(x, nblag.i, style="B", method="randomization", alternative=alternative, diag.zero=diag.zero)$statistic
		}
		global.sas.p.vec <- cbind(global.sas.p.vec, global.sas.p.i)
	}

	plot(0:k, global.sas.p.vec, type="b", main="Spatial Correlogram", xlab="Spatial Lags", ylab="Moran's I (Z-values)", lwd=1)	
	axis(1,0:k)
#	plot(1:k, global.sas.p.vec[-1], type="b", xlab="Spatial Lags", ylab="Moran's I (Z-values)", lwd=1)	
	abline(h=2, lty=8, lwd=1)
	abline(h=-2, lty=8, lwd=1)
	invisible()
}

spatial.cross.correlogram <- function (x, y, nb, maxsr, type.higher.nb="exclusive", type.row.stand="regular", alternative="two", diag.zero=TRUE)
{
  # Purpose: Drawing cross-correlograms
  # Arguments: 
  #   x: a vector of one variable
  #   y: a vector of another variable
  #   nb: nb object
  #   style <- match.arg(style, c("B","C","S","W","U"))
  #   maxsr: the maximum number of spatial ranges
  #   type.higher.nb: types of making higher-order nb objects 	
  #   type.row.stand: types of doing row-standardization of SPM 	
  #   alternative: type of test
  #   diag.zero: if FALSE, each region has itself as its neighbour
  # Fuction called:
  #   nb.self
  #   nblag.generator
  #   test.global.pearson
  #   test.global.bMoran

  alternative <- match.arg(alternative, c("greater", "less", "two.sided"))
# style <- match.arg(style, c("B","C","S","W","U"))
  type.higher.nb <- match.arg(type.higher.nb, c("inclusive","exclusive"))
  type.row.stand <- match.arg(type.row.stand, c("regular","special", "none"))
  mf <- match.call(expand.dots = FALSE)

	n <- length(x)
	maxsr.sg <- round(sqrt(n)/2) 
	if (maxsr > maxsr.sg) maxsr <- maxsr.sg

	global.pearson.p <- test.global.pearson(x, y, alternative=alternative)$statistic
	global.sas.p.vec <- as.vector(global.pearson.p)
	
	if (type.higher.nb=="inclusive") {
	nblag.list <- nblag.generator(nb, maxlag=maxsr, type="inclusive")
      }
	else if (type.higher.nb=="exclusive") {
	nblag.list <- nblag.generator(nb, maxlag=maxsr, type="exclusive")
      }

	k <- maxsr
	for (i in 1:k)
	{
		nblag.i <- nblag.list[[i]]
		if (type.row.stand=="special")
		{
		global.sas.p.i <- test.global.bMoran(x, y, nblag.i, style="V", alternative=alternative, diag.zero=diag.zero)$statistic
		}
		else if (type.row.stand=="regular")
		{
		global.sas.p.i <- test.global.bMoran(x, y, nblag.i, style="W", alternative=alternative, diag.zero=diag.zero)$statistic
		}
		else if (type.row.stand=="none")
		{
		global.sas.p.i <- test.global.bMoran(x, y, nblag.i, style="B", alternative=alternative, diag.zero=diag.zero)$statistic
		}
		global.sas.p.vec <- cbind(global.sas.p.vec, global.sas.p.i)
	}

	plot(0:k, global.sas.p.vec, type="b", main="Spatial Cross-correlogram", xlab="Spatial Lags", ylab="Bivariate Moran (Z-values)", lwd=1)	
	axis(1,0:k)
#	plot(1:k, global.sas.p.vec[-1], type="b", xlab="Spatial Lags", ylab="Bivariate Moran (Z-values)", lwd=1)	
	abline(h=2, lty=8, lwd=1)
	abline(h=-2, lty=8, lwd=1)
	invisible()
}

