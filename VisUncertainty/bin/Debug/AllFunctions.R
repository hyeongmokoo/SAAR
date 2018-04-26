#Function for local version correlogram
diff.lag.listw <- function(listw, var){
  n <- length(listw$neighbours) 
  cardnb <- card(listw$neighbours)
  #res <- matrix(nrow = n, ncol = 1)
  res <- vector(mode = "numeric", length = n)
  for(i in 1:n)
  {
    nbs <- cardnb[i]
    diff.sum <- 0
    for(j in 1:nbs)
    {
      #diff <- var[i] - var[listw$neighbours[[i]][j]]
      diff <- (var[i] - var[listw$neighbours[[i]][j]])^2

      #diff <- var[listw$neighbours[[i]][j]]-var[i]
      diff.sum <- diff.sum + diff
    }
    #res[i] <- diff.sum / nbs
    #res[i] <- (sqrt(diff.sum) / nbs)/sd(var)
    res[i] <- (sqrt(diff.sum) / nbs)
  }
  res
}

localgeary <- function(x, listw) ##Function from S-I Lee
{
  n <- length(listw$neighbours)
  cardnb <- card(listw$neighbours)
  tss <- sum((x-mean(x))^2)
  #	dev.x <- x-mean(x)
  sum.v <- sum(cardnb)
  
  res <- matrix(nrow = n, ncol = 1)
  
  for(i in 1:n)
  {
    nbs <- cardnb[i]
    diff.sum <- 0
    
    for(j in 1:nbs){
      diff <- (x[i] - x[listw$neighbours[[i]][j]])^2
      diff.sum <- diff.sum + diff
    }
    res[i,1] <- ((n*(n-1))/(2*sum.v))*(diff.sum/tss)
  }
  res
}


localgeary2 <- function(x, listw){ ##This function is wrong.
  scale.x <- scale(x)   
  n <- length(listw$neighbours) 
  cardnb <- card(listw$neighbours)
  res <- matrix(nrow = n, ncol = 1)
  for(i in 1:n)
  {
    nbs <- cardnb[i]
    diff.sum <- 0
    for(j in 1:nbs)
    {
      diff <- (scale.x[i] - scale.x[listw$neighbours[[i]][j]])^2
      diff.sum <- diff.sum + diff
    }
    res[i,1] <- diff.sum / nbs
    #res[i,2] <- (2 * n)/(n-1)
  }
  res
}
