rm(list=ls(all=TRUE)) 

library(maptools)
library(spdep)

setwd("D:\\Dropbox\\Projects\\2014\\NIH_withDrChun_DrGriffith\\Programmings\\VisUncertainty\\VisUncertainty\\bin\\Debug\\SampleData\\Texas") 
sample.shp <- readShapePoly("tl_2014_tx_cnt_SP.shp")
sample.df <- as.data.frame(sample.shp)
sample.nb <- poly2nb(sample.shp)

sample.est <- sample.df$medinc
sample.var <- sample.df$mi_se

sample.result <- UncernSAM(sample.est, sample.var, sample.nb)

nb <- sample.nb
x.e <- sample.est
x.v <- sample.var
listw <- sample.listw
sn <- listw2sn(listw)
V <- as(as(as_dgRMatrix_listw(listw), "RsparseMatrix"), "CsparseMatrix")
nw <- sum(rowSums(V))

#d.x <- x - mean(x)
#z.x <- d.x/(sqrt(sum(d.x^2)/n))
((n-1)/(2*nw))



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
  
  res <- list(res.bd, res.bc, res.hd)
  res
}

boxplot(res)
plot(x.e, res.bc)

sample.bbox <- bbox(sample.shp)  
plot(sample.shp,axes=T,col=grey(0.9),border="white",
     xlim=sample.bbox[1,],ylim=sample.bbox[2,])               # first background


plotColorRamp(res,sample.shp,n.breaks=6,    # second add map
              my.title="",
              my.legend="Bc",addToMap=T) 

hist(res)

mod.c <- sn[,3]*Bhatta.dist(x.e[sn[,1]], x.e[sn[,2]], x.v[sn[,1]], x.v[sn[,2]])

mod.c <- ((n-1)/(2*nw))*sum(sn[,3]*(x[sn[,1]]-x[sn[,2]])^2)

mapping.seq(sample.shp, mod.c, 6, vec.int)


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

mapping.seq <- function(polys, x, nclass, predifendBreaks){
  require(RColorBrewer)
  require(classInt)
  
  pal.red <- brewer.pal(nclass, "Reds")
  q.n <- classIntervals(x, nclass, style="fixed", fixedBreaks=predifendBreaks, intervalClosure="right") 
  cols.red <- findColours(q.n, pal.red)
  plot(sample.shp, col=cols.red)
  brks <- round(q.n$brks,3)
  leg <- paste(brks[-(nclass+1)], brks[-1], sep=" - ")
  legend("bottomright", fill=pal.red, legend=leg, bty="n")
}

plotColorRamp <- function(var.name,shape,n.breaks=8,my.title="",
                          my.legend=deparse(substitute(var.name)),
                          addToMap=F) {
  ##
  ## Plot a color ramp variable "var.name"
  ##
  require(spdep); require(RColorBrewer); require(classInt)
  
  ## define breaks and color assignment
  q.breaks <- classIntervals(var.name, n=n.breaks, style="quantile")
  pal.YlOrRd <- brewer.pal(n.breaks, "YlOrRd")
  map.col <- pal.YlOrRd[findInterval(var.name,q.breaks$brks,rightmost.closed=T)]
  ## generate choropleth map
  plot(shape,col=map.col,border=grey(0.9),axes=T,add=addToMap)
  legend("bottomleft", title=my.legend,legend=leglabs(round(q.breaks$brks,digits=2)),
         fill=pal.YlOrRd,bty="n",ncol=1)
  title(my.title)
  box()
} # end:plotColorRamp 
