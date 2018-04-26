nb.self <- function (nb) {
  # purpose: Making non-zero diagonal elements in spatial proximity matrix
  # Arguments:  
  #   nb: nb object
  # Author: Yongwan Chun	

  n <- length(nb)
  for (i in 1:n) nb[[i]] <- sort(c(i,nb[[i]]))
  return(nb)
}

nblag.generator <- function(nb, maxlag, type="inclusive") {
  # purpose: Constructing higher-order spatial lags with self excluded
  # Arguments:  
  #   nb: nb object
  #   maxlag: maximum-order number for defining neighbors
  #   type: types of making higher-order nb objects
  # Author: Hyeongmo Koo	

  type <- match.arg(type, c("exclusive","inclusive"))
 
  if (type=="inclusive") {
  lag.list <- nblag(nb, maxlag=maxlag)
  nlags <- length(lag.list)
  n <- length(lag.list[[1]])
  for(i in 2:nlags){
      for(j in 1:n){
        lag.list[[i]][[j]] <- as.integer(sort(c(lag.list[[i-1]][[j]],lag.list[[i]][[j]])))
    }
  }
   lag.list.inclusive <- lag.list 
  }
  else if (type=="exclusive") {
  lag.list.exclusive <- nblag(nb, maxlag=maxlag)
  }
}

nblag.generator.self <- function(nb, maxlag, type="inclusive") {
  # purpose: Constructing higher-order spatial lags with self included
  # Arguments:  
  #   nb: nb object
  #   maxlag: maximum-order number for defining neighbors
  #   type: types of making higher-order nb objects
  # Author: Hyeongmo Koo	

  type <- match.arg(type, c("exclusive","inclusive"))
  
  if (type=="inclusive") {
  lag.list <- nblag(nb, maxlag=maxlag)
  nlags <- length(lag.list)
  n <- length(lag.list[[1]])
   for(i in 1:nlags){
    if(i==1){
      for(j in 1:n){
        lag.list[[i]][[j]] <- as.integer(sort(c(lag.list[[i]][[j]], j)))
      }
    }
    else{
      for(j in 1:n){
        lag.list[[i]][[j]] <- as.integer(sort(c(lag.list[[i-1]][[j]],lag.list[[i]][[j]])))
      }
    }
  }
  lag.list.inclusive <- lag.list      
  }
  else if (type=="exclusive") {
  lag.list.exclusive <- nblag(nb, maxlag=maxlag)
  }
}

nb2listv <- function (nb, beta=0.5){
  # purpose: Transforming a SPM according to a special row-standardization
  # Arguments:  
  #   nb: nb object
  #   beta: a weight allocated to a central location
  # Author: Hyeongmo Koo	

  style <- 'V'
  neighbours <- nb
  n <- length(neighbours)
  cardnb <- card(neighbours)
  vlist <- vector(mode = "list", length = n)
  glist <- vector(mode = "list", length = n)
  for (i in 1:n) if (cardnb[i] > 0) {
    glist[[i]] <- rep(1, length = cardnb[i])
    mode(glist[[i]]) <- "numeric"
  }
  d <- unlist(lapply(glist, sum))-1
  for (i in 1:n) {
    if (cardnb[i] > 0) {
      if (d[i] > 0) {
        vlist[[i]] <- ((1-beta)/d[i]) * glist[[i]]
        self.idx <- which(neighbours[[i]]==i)
        vlist[[i]][self.idx] <- beta
      }
      else vlist[[i]] <- 0 * glist[[i]]
    }
  }
  attr(vlist, "comp") <- list(d = d)
  res <- list(style = style, neighbours = neighbours, weights = vlist)
  class(res) <- c("listw", "nb")
  attr(res, "region.id") <- attr(neighbours, "region.id")
  attr(res, "call") <- match.call()
  res
} 

