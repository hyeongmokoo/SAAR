# Program Purpose: 
#   Conducting the LARRY (L And R based Reseearch methodologY).
# Reference:
#   Sang-Il Lee (2017) "Conceptualizing and exploring bivariate spatial dependence
#   and heterogeneity: A bivariate ESDA-LISA research framework", under review.
# Date: 2017. 3. 16.
# Author: Sang-Il Lee (with help of Yongwan Chun and Hyeongmo Koo)

LARRY.bivariate.LISA.lee <- function(x, y, id, nb, style="W", diag.zero=FALSE)
{
  # purpose: Drawing a simple choropleth map with local Lee's L statistics
  # Arguments:
  #   x: a vector of one variable
  #   y: a vector of another variable
  #   nb: nb object
  #   style: style of spatial weight matrix (refer to nb2listw)
  #   diag.zero: if FALSE, each region has itself as its neighbour
  # Fuction called:
  #   nb.self
  #   local.lee.L
 
  res <- local.lee.L(x, y, id, nb, style, diag.zero)
  return(res)
}

LARRY.bivariate.LISA.pearson <- function(x, y, id)
{
  # purpose: Drawing a simple choropleth map with local Pearson's ri
  # Arguments:
  #   x: a vector of one variable
  #   y: a vector of another variable
  #   id: a vector of ID codes
  # Fuction called:
  #   local.pearson

  res <- local.pearson(x, y, id)
  return(res)
}

LARRY.bivariate.quadrant.lee <- function(x, y, id, nb, style="W", diag.zero=FALSE)
{
  # purpose: Drawing a bivariate spatial quadrant map with local Lee's L statistics
  # Arguments:
  #   x: a vector of one variable
  #   y: a vector of another variable
  #   nb: nb object
  #   style: style of spatial weight matrix (refer to nb2listw)
  #   diag.zero: if FALSE, each region has itself as its neighbour
  # Fuction called:
  #   nb.self
  #   local.lee.L
 
  res <- local.lee.L(x, y, id, nb, style, diag.zero)
  return(res)
}

LARRY.bivariate.quadrant.pearson <- function(x, y, id)
{
  # purpose: Drawing a simple choropleth map with local Pearson's ri
  # Arguments:
  #   x: a vector of one variable
  #   y: a vector of another variable
  #   id: a vector of ID codes
  # Fuction called:
  #   local.pearson

  res <- local.pearson(x, y, id)
  return(res)
}

LARRY.bivariate.probability.lee <- function(x, y, id, nb, style="W", sig.levels=c(0.05), method="total", alternative="two.sided", diag.zero=FALSE)
{
  # Purpose: Drawing a simple choropleth map with p-values of local Lee's L statistics
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
  #   test.local.lee.L

  res <- test.local.lee.L (x, y, id, nb, style, sig.levels, method, alternative, diag.zero)
  res <- data.frame(id=res$id, local.lee=res$local.L, z.value=res$Z, p.value=res$P)
  return(res)
}

LARRY.bivariate.probability.pearson <- function(x, y, id, sig.levels=c(0.05, 0.01), method="randomization", alternative="two.sided")
{
  # Purpose: Drawing a simple choropleth map with p-values of local Pearson statistics
  # Arguments:
  #   x: a vector of one variable
  #   y: a vector of another variable
  #   id: a vector of ID codes
  #   sig.levels: a vector of significance levels	
  #   method: "randomization" or "normality" assumption for null hypothesis
  #   alternatie: type of test
  res <- test.local.pearson (x, y, id, sig.levels, method, alternative)
  res <- data.frame(id=res$id, pearson.r=res$pearson.r, z.value=res$Z, p.value=res$P)
  return(res)
}

LARRY.bivariate.sig.quad.lee <- function(x, y, id, nb, style="W", sig.levels=c(0.05), method="total", alternative="two.sided", diag.zero=FALSE)
{
  # Purpose: Drawing a bivariate spatial quadrant map in conjunction with p-values of local Lee's L statistics
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
  #   test.local.lee.L
  
  sig.n <- length(sig.levels)
  res <- test.local.lee.L (x, y, id, nb, style, sig.levels, method, alternative, diag.zero)
  res <- data.frame(id=res$id, local.lee=res$local.L, z.value=res$Z, p.value=res$P, quad=res$quad, res[13:(13+sig.n-1)])
  return(res)
}

LARRY.bivariate.sig.quad.pearson <- function(x, y, id, sig.levels=c(0.05, 0.01), method="randomization", alternative="two.sided")
{
  # Purpose: Drawing a bivariate spatial quadrant map in conjunction with p-values of local Pearson statistics
  # Arguments:
  #   x: a vector of one variable
  #   y: a vector of another variable
  #   id: a vector of ID codes
  #   sig.levels: a vector of significance levels	
  #   method: "randomization" or "normality" assumption for null hypothesis
  #   alternatie: type of test

  sig.n <- length(sig.levels)
  res <- test.local.pearson (x, y, id, sig.levels, method, alternative)
  res <- data.frame(id=res$id, pearson.r=res$pearson.r, z.value=res$Z, p.value=res$P, quad=res$quad, res[11:(11+sig.n-1)])
  return(res)
}

LARRY.bivariate.spatial.range <- function (x, y, id, nb, maxsr, pearson.sig=0.05, lee.sig=0.05, method.pearson="randomization", method.lee="total", type.higher.nb="include", type.row.stand="regular", alternative="two", diag.zero=FALSE)
{
  # Purpose: Drawing a bivariate spatial range map using Lee's statistics
  # Arguments: 
  #   x: a vector of one variable
  #   y: a vector of another variable
  #   id: a vector of ID codes
  #   nb: nb object
  #   maxsr: the maximum number of spatial ranges
  #   pearson.sig: a significance level for local Pearson
  #   lee.sig: a significance level for local Lee's Li*
  #   method.lee: type of significance testing for local Lee's Li*
  #   type.higher.nb: types of making higher-order nb objects 	
  #   type.row.stand: types of doing row-standardization of SPM 	
  #   alternative: type of test
  #   diag.zero: if FALSE, each region has itself as its neighbour
  # Fuction called:
  #   test.local.lee.L
  #   test.local.pearson

	n <- length(x)

	local.pearson.res <- test.local.pearson(x, y, id, pearson.sig, method=method.pearson, alternative)
	local.pearson.vec <- local.pearson.res$pearson.r
	local.pearson.p.vec <- local.pearson.res$P
	local.pearson.quad.vec <- local.pearson.res$quad	

	local.sas.dtfr <- data.frame(local.pearson.vec)
	local.sas.p.dtfr <- data.frame(local.pearson.p.vec)
	local.sas.quad.dtfr <- data.frame()
	local.sas.quad.dtfr[1:n,1] <- local.pearson.quad.vec

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
		local.sas.res <- test.local.lee.L(x, y, id, nblag.i, style="V", sig.levels=lee.sig, method=method.lee, alternative, diag.zero)
		}
		else if (type.row.stand=="regular")
		{
		local.sas.res <- test.local.lee.L(x, y, id, nblag.i, style="W", sig.levels=lee.sig, method=method.lee, alternative, diag.zero)
		}
		else if (type.row.stand=="none")
		{
		local.sas.res <- test.local.lee.L(x, y, id, nblag.i, style="B", sig.levels=lee.sig, method=method.lee, alternative, diag.zero)
		}

		local.sas.p.dtfr[1:n,i+1] <- as.vector(local.sas.res$P)
		local.sas.quad.dtfr[1:n,i+1] <- as.character(local.sas.res$quad)
		local.sas.dtfr[1:n,i+1] <- as.vector(local.sas.res$local.L)
	}
	max.spatial.range.vec <- vector()
	p.vec <- vector()
	sas.vec <- vector()
	quad.vec <- vector()
	for(j in 1:n)
	{
  # 		max.spatial.range.j <- as.numeric(which(local.sas.p.dtfr[j,]==min(local.sas.p.dtfr[j,]))[1])
		max.spatial.range.j <- as.numeric(which.min(local.sas.p.dtfr[j,]))
		max.spatial.range.vec[j] <- max.spatial.range.j
		p.min.j <- local.sas.p.dtfr[j,max.spatial.range.j]
		p.vec[j] <- p.min.j
		sas.j <- local.sas.dtfr[j,max.spatial.range.j]
		sas.vec[j] <- sas.j
		quad.j <- as.character(local.sas.quad.dtfr[j,max.spatial.range.j])
		quad.vec[j] <- quad.j
	}
	max.spatial.range.vec <- max.spatial.range.vec-1
	names(local.sas.p.dtfr) <- paste("sr.", 0:maxsr, sep="")
	max.quad.dtfr <- data.frame()
	for (k in 1:n)
	{
		for (m in 0:maxsr)	
		{
			if (max.spatial.range.vec[k]==m)
			{
				max.quad.dtfr[k,m+1] <- quad.vec[k]
			}
			else {max.quad.dtfr[k,m+1] <- as.character("N/A")}
		}
	}
	names(max.quad.dtfr) <- paste("sr.quad", 0:maxsr, sep="")
	final.max.sr <- paste(quad.vec, max.spatial.range.vec, sep="")
#	res <- data.frame(id=id, maxsr.sas=sas.vec, maxsr.p=p.vec, maxsr=max.spatial.range.vec, max.quad=quad.vec, final.max.sr, positive.sr=positive.sr, negative.sr=negative.sr, max.quad.dtfr, local.sas.p.dtfr )
	res <- data.frame(id=id, maxsr.sas=sas.vec, maxsr.p=p.vec, maxsr=max.spatial.range.vec, max.quad=quad.vec, final.max.sr)
	return(res)
}


LARRY.bivariate.spatial.cluster <- function(x, y, id, nb, global.sig=0.05, method="total", type.row.stand="regular", alternative="two", diag.zero=FALSE)
{
  # Purpose: Drawing a bivariate spatial cluster map using Lee's L statistics
  # Arguments: 
  #   x: a vector of one variable
  #   y: a vector of another variable
  #   id: a vector of ID codes
  #   nb: nb object
  #   global.sig: global significance level
  #   method: type of significance testing for local Lee's Li*
  #   alternative: type of test
  #   type.row.stand: types of doing row-standardization of SPM
  #   diag.zero: if FALSE, each region has itself as its neighbour
  # Fuction called:
  #   test.local.lee.L

  alternative <- match.arg(alternative, c("greater", "less", "two.sided"))
# style <- match.arg(style, c("B","C","S","W","U"))
  method <- match.arg(method, c("total","conditional"))
  type.row.stand <- match.arg(type.row.stand, c("regular","special", "none"))
  mf <- match.call(expand.dots = FALSE)
	
	n <- length(x)

	gen.Bonf <- global.sig/n
	sp.Bonf.L <- global.sig/(n^2/sum(card(nb)))

	if (type.row.stand=="special")
	{
	res.lee.L <- test.local.lee.L(x, y, id, nb, style="V", sig.levels=global.sig, method=method, alternative=alternative, diag.zero=diag.zero)
	}
	else if (type.row.stand=="regular")
	{
	res.lee.L <- test.local.lee.L(x, y, id, nb, style="W", sig.levels=global.sig, method=method, alternative=alternative, diag.zero=diag.zero)
	}
	else if (type.row.stand=="none")
	{
	res.lee.L <- test.local.lee.L(x, y, id, nb, style="B", sig.levels=global.sig, method=method, alternative=alternative, diag.zero=diag.zero)
	}

	P.L.vec <- res.lee.L$P
 	FDR.L <- global.sig*(length(P.L.vec[P.L.vec<global.sig])/n)

#	sp.Bonf.r <- global.sig/n
#	res.pearson.r <- test.local.pearson(x, y, id, sig.levels=global.sig, method=method, alternative)
#	P.r.vec <- res.pearson.r$P
#	FDR.r <- global.sig*(length(P.r.vec[P.r.vec<global.sig])/n)
#	all.sig.r <- c(global.sig, FDR.r, sp.Bonf.r, gen.Bonf)
#	final.res.pearson.r <- test.local.pearson(x, y, id, sig.levels=all.sig.r, method=method, alternative=alternative)

	all.sig.L <- c(global.sig, FDR.L, sp.Bonf.L, gen.Bonf)
	if (type.row.stand=="special")
	{
	final.res.lee.L <- test.local.lee.L(x, y, id, nb, style="V", sig.levels=all.sig.L, method=method, alternative, diag.zero)
	}
	else if (type.row.stand=="regular")
	{
	final.res.lee.L <- test.local.lee.L(x, y, id, nb, style="W", sig.levels=all.sig.L, method=method, alternative, diag.zero)
	}
	else if (type.row.stand=="none")
	{
	final.res.lee.L <- test.local.lee.L(x, y, id, nb, style="B", sig.levels=all.sig.L, method=method, alternative, diag.zero)
	}
	
	sig.names <- c("sig.global", "sig.FDR", "sig.spBonf", "sig.genBonf")	
#	res <- data.frame(final.res.lee.L, pearson=final.res.pearson.r$pearson.r, pearson.P=final.res.pearson.r$P, pearson.quad=final.res.pearson.r$quad, final.res.pearson.r[,12:15])
	res <- final.res.lee.L
	colnames(res)[13:16] <- sig.names
      names(all.sig.L) <- sig.names
	res.list <- list(results=res, sig.levels=all.sig.L)
      return(res.list)
}


LARRY.bivariate.spatial.outlier <- function (x, y, id, nb, pearson.sig=0.05, lee.sig=0.05, method.pearson="randomization", method.lee="total", type.row.stand="regular", alternative="two", diag.zero=FALSE)
{
  # Purpose: Drawing a bivariate spatial outlier map using Lee's L and local Pearson's r
  # Arguments: 
  #   x: a vector of one variable
  #   y: a vector of another variable
  #   id: a vector of ID codes
  #   nb: nb object
  #   pearson.sig: a significance level for local Pearson
  #   lee.sig: a significance level for local Lee's Li*
  #   method.pearson: type of significance testing for local Pearson
  #   method.lee: type of significance testing for local Lee's Li*
  #   alternative: type of test
  #   type.row.stand: types of doing row-standardization of SPM
  #   diag.zero: if FALSE, each region has itself as its neighbour
  # Fuction called:
  #   test.local.lee.L
  #   test.local.pearson

  alternative <- match.arg(alternative, c("greater", "less", "two.sided"))
# style <- match.arg(style, c("B","C","S","W","U"))
  method.pearson <- match.arg(method.pearson, c("normality","randomization"))
  method.lee <- match.arg(method.lee, c("total","conditional"))
  type.row.stand <- match.arg(type.row.stand, c("regular","special", "none"))
  mf <- match.call(expand.dots = FALSE)
	
	pearson.res <- test.local.pearson(x, y, id, pearson.sig, method=method.pearson, alternative)
	if (type.row.stand=="special")
	{
	res.lee.L <- test.local.lee.L(x, y, id, nb, style="V", sig.levels=lee.sig, method=method.lee, alternative, diag.zero)
	}
	else if (type.row.stand=="regular")
	{
	res.lee.L <- test.local.lee.L(x, y, id, nb, style="W", sig.levels=lee.sig, method=method.lee, alternative, diag.zero)
	}
	else if (type.row.stand=="none")
	{
	res.lee.L <- test.local.lee.L(x, y, id, nb, style="B", sig.levels=lee.sig, method=method.lee, alternative, diag.zero)
	}
	spatial.outlier.vec <- as.vector(pearson.res$quad)
	spatial.outlier.vec[res.lee.L$P > lee.sig] <- as.character("not sig.")
	spatial.outlier.vec[pearson.res$quad==res.lee.L$quad] <- as.character("not sig.")
	spatial.outlier.vec.sig <- spatial.outlier.vec
	spatial.outlier.vec.sig[pearson.res$P > pearson.sig] <- as.character("not sig.")
	
#	lee.spatial.outlier <- data.frame(id=id, z.x=res.lee.L$z.x, z.y=res.lee.L$z.y, v.z.x=res.lee.L$v.z.x, v.z.y=res.lee.L$v.z.y, pearson.r=pearson.res$pearson.r, local.lee=res.lee.L$local.lee, 
#		pearson.quad=pearson.res$quad, lee.quad=res.lee.L$quad, p.pearson=pearson.res$P, p.lee=res.lee.L$P, sp.outlier=spatial.outlier.vec, sp.outlier.sig=spatial.outlier.vec.sig)		
	lee.spatial.outlier <- data.frame(id=id, pearson.r=pearson.res$pearson.r, local.L=res.lee.L$local.L, 
		pearson.quad=pearson.res$quad, lee.quad=res.lee.L$quad, p.pearson=pearson.res$P, p.lee=res.lee.L$P, sp.outlier=spatial.outlier.vec, sp.outlier.sig=spatial.outlier.vec.sig)		
	return(lee.spatial.outlier)
}


