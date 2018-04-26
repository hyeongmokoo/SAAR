# Program Purpose: 
#   Conducting the SARRY (S And R based Reseearch methodologY).
# Reference:
# Date: 2017. 3. 16.
# Author: Sang-Il Lee (with help of Yongwan Chun and Hyeongmo Koo)

SARRY.univariate.spatial.cluster <- function(x, id, nb, global.sig=0.05, method.z2="randomization", method.lee="total", type.row.stand="regular", alternative="two", diag.zero=FALSE)
{
  # Purpose: Univariate spatial cluster analysis using SARRY
  # Arguments: 
  #   x: a vector of one variable
  #   id: a vector of ID codes
  #   nb: nb object
  #   global.sig: global significance level
  #   method.pearson: type of significance testing for local Pearson
  #   method.lee: type of significance testing for local Lee's Li*
  #   alternative: type of test
  #   type.row.stand: types of doing row-standardization of SPM
  #   diag.zero: if FALSE, each region has itself as its neighbour
  # Fuction called:
  #   test.local.lee.S
  #   test.local.z2

  alternative <- match.arg(alternative, c("greater", "less", "two.sided"))
# style <- match.arg(style, c("B","C","S","W","U"))
  method.z2 <- match.arg(method.z2, c("normality","randomization"))
  method.lee <- match.arg(method.lee, c("total","conditional"))
  type.row.stand <- match.arg(type.row.stand, c("regular","special", "none"))
  mf <- match.call(expand.dots = FALSE)
	
	n <- length(x)

	gen.Bonf <- global.sig/n
	sp.Bonf.S <- global.sig/(n^2/sum(card(nb)))

	if (type.row.stand=="special")
	{
	res.lee.S <- test.local.lee.S(x, id, nb, style="V", sig.levels=global.sig, method=method.lee, alternative=alternative, diag.zero=diag.zero)
	}
	else if (type.row.stand=="regular")
	{
	res.lee.S <- test.local.lee.S(x, id, nb, style="W", sig.levels=global.sig, method=method.lee, alternative=alternative, diag.zero=diag.zero)
	}
	else if (type.row.stand=="none")
	{
	res.lee.S <- test.local.lee.S(x, id, nb, style="B", sig.levels=global.sig, method=method.lee, alternative=alternative, diag.zero=diag.zero)
	}

	P.S.vec <- res.lee.S$P
 	FDR.S <- global.sig*(length(P.S.vec[P.S.vec<global.sig])/n)

	sp.Bonf.z2 <- global.sig/n
	res.z2 <- test.local.z2(x, id, sig.levels=global.sig, method=method.z2, alternative=alternative)
	P.z2.vec <- res.z2$P
	FDR.z2 <- global.sig*(length(P.z2.vec[P.z2.vec<global.sig])/n)
	all.sig.S <- c(global.sig, FDR.S, sp.Bonf.S, gen.Bonf)
	if (type.row.stand=="special")
	{
	final.res.lee.S <- test.local.lee.S(x, id, nb, style="V", sig.levels=all.sig.S, method=method.lee, alternative=alternative, diag.zero=diag.zero)
	}
	else if (type.row.stand=="regular")
	{
	final.res.lee.S <- test.local.lee.S(x, id, nb, style="W", sig.levels=all.sig.S, method=method.lee, alternative=alternative, diag.zero=diag.zero)
	}
	else if (type.row.stand=="none")
	{
	final.res.lee.S <- test.local.lee.S(x, id, nb, style="B", sig.levels=all.sig.S, method=method.lee, alternative=alternative, diag.zero=diag.zero)
	}
	all.sig.z2 <- c(global.sig, FDR.z2, sp.Bonf.z2, gen.Bonf)
	final.res.z2 <- test.local.z2(x, id, sig.levels=all.sig.z2, method=method.z2, alternative=alternative)
	res <- data.frame(final.res.lee.S, local.z2=final.res.z2$local.z2, z2.P=final.res.z2$P, z2.quad=final.res.z2$quad, final.res.z2[,10:14])
      return(res)
}

SARRY.univariate.spatial.outlier <- function (x, id, nb, z2.sig=0.05, lee.sig=0.05, method.z2="randomization", method.lee="total", type.row.stand="regular", alternative="two", diag.zero=FALSE)
{
  # Purpose: Bivariate spatial outlier analysis using LARRY
  # Arguments: 
  #   x: a vector of one variable
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
  method.z2 <- match.arg(method.z2, c("normality","randomization"))
  method.lee <- match.arg(method.lee, c("total","conditional"))
  type.row.stand <- match.arg(type.row.stand, c("regular","special", "none"))
  mf <- match.call(expand.dots = FALSE)
	
	z2.res <- test.local.z2(x, id, z2.sig, method=method.z2, alternative=alternative)
	if (type.row.stand=="special")
	{
	res.lee.S <- test.local.lee.S(x, id, nb, style="V", sig.levels=lee.sig, method=method.lee, alternative=alternative, diag.zero=diag.zero)
	}
	else if (type.row.stand=="regular")
	{
	res.lee.S <- test.local.lee.S(x, id, nb, style="W", sig.levels=lee.sig, method=method.lee, alternative=alternative, diag.zero=diag.zero)
	}
	else if (type.row.stand=="none")
	{
	res.lee.S <- test.local.lee.S(x, id, nb, style="B", sig.levels=lee.sig, method=method.lee, alternative=alternative, diag.zero=diag.zero)
	}
	spatial.outlier.vec <- as.vector(z2.res$quad)
	spatial.outlier.vec[res.lee.S$P > lee.sig] <- as.character("Not significant")
	spatial.outlier.vec[z2.res$quad==res.lee.S$quad] <- as.character("Not significant")
	spatial.outlier.vec.sig <- spatial.outlier.vec
	spatial.outlier.vec.sig[z2.res$P > z2.sig] <- as.character("Not significant")
	
#	lee.spatial.outlier <- data.frame(id=id, z.x=res.lee.S$z.x, z.y=res.lee.L$z.y, v.z.x=res.lee.L$v.z.x, v.z.y=res.lee.L$v.z.y, local.z2=z2.res$local.z2, local.lee=res.lee.S$local.lee, 
#		z2.quad=z2.res$quad, lee.quad=res.lee.S$quad, p.z2=z2.res$P, p.lee=res.lee.S$P, sp.outlier=spatial.outlier.vec, sp.outlier.sig=spatial.outlier.vec.sig)		
	lee.spatial.outlier <- data.frame(id=id, local.z2=z2.res$local.z2, lee.S=res.lee.S$lee.S, 
		z2.quad=z2.res$quad, lee.quad=res.lee.S$quad, p.z2=z2.res$P, p.lee=res.lee.S$P, sp.outlier=spatial.outlier.vec, sp.outlier.sig=spatial.outlier.vec.sig)		
	return(lee.spatial.outlier)
}

SARRY.univariate.spatial.range <- function (x, id, nb, maxsr, z2.sig=0.05, lee.sig=0.05, method.z2="randomization", method.lee="total", type.higher.nb="inclusive", type.row.stand="regular", alternative="two", diag.zero=FALSE)
{

  # Purpose: Univariate spatial range analysis using SARRY
  # Arguments: 
  #   x: a vector of one variable
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

	local.z2.res <- test.local.z2(x, id, z2.sig, method=method.z2, alternative=alternative)
	local.z2.vec <- local.z2.res$local.z2
	local.z2.p.vec <- local.z2.res$P
	local.z2.quad.vec <- local.z2.res$quad	

	local.sas.dtfr <- data.frame(local.z2.vec)
	local.sas.p.dtfr <- data.frame(local.z2.p.vec)
	local.sas.quad.dtfr <- data.frame()
	local.sas.quad.dtfr[1:n,1] <- local.z2.quad.vec

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
		local.sas.res <- test.local.lee.S(x, id, nblag.i, style="V", sig.levels=lee.sig, method=method.lee, alternative=alternative, diag.zero=diag.zero)
		}
		else if (type.row.stand=="regular")
		{
		local.sas.res <- test.local.lee.S(x, id, nblag.i, style="W", sig.levels=lee.sig, method=method.lee, alternative=alternative, diag.zero=diag.zero)
		}
		else if (type.row.stand=="none")
		{
		local.sas.res <- test.local.lee.S(x, id, nblag.i, style="B", sig.levels=lee.sig, method=method.lee, alternative=alternative, diag.zero=diag.zero)
		}

		local.sas.p.dtfr[1:n,i+1] <- as.vector(local.sas.res$P)
		local.sas.quad.dtfr[1:n,i+1] <- as.character(local.sas.res$quad)
		local.sas.dtfr[1:n,i+1] <- as.vector(local.sas.res$lee.S)
	}
	max.spatial.range.vec <- vector()
	p.vec <- vector()
	sas.vec <- vector()
	quad.vec <- vector()
	for(j in 1:n)
	{
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

