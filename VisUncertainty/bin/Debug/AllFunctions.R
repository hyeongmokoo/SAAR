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

stepwise <- function(full, initial, sle=0.1, sls=0.1001, verbose=T) {
  cut.string <- function(string) {
    if (length(string) > 1L)
      string[-1L] <- paste("\n", string[-1L], sep = "")
    string
  }
  msef <- sum(full$residuals^2)/full$df
  n <- length(full$residuals)
  allvars <- attr(full$terms, "predvars")
  current <- initial
  while (TRUE) {
    temp <- summary(current)
    rnames <- names(current$coefficient)
    if (verbose) print(current$coefficients)
    p <- current$rank
    mse <- sum(current$residuals^2)/current$df
    if (verbose) {
      cat("S = ", temp$sigma, " R-sq = ", temp$r.squared, "\n\n")
    }
    
    if (p > initial$rank) {  
      d <- drop1(current, test="F")
      pmax <- max(d[-c(1:initial$rank),6])
      if (pmax > sls) {
        var.del <- rownames(d)[d[,6] == pmax];  # name of variable to delete
        if (length(var.del) > 1) var.del <- var.del[2]			
        current <- update(current, paste("~ . - ", var.del), evaluate=FALSE)
        current <- eval.parent(current)
        if (verbose) {
          cat("\nStep:  \n", cut.string(deparse(as.vector(formula(current)))), "\n\n", sep = "")
          utils::flush.console()
        }
        next
      }
    }
    
    a <- tryCatch(add1(current, scope=full, test="F"), error=function(e) NULL);
    if (is.null(a)) break;
    
    pmin <- min(a[-1,6])
    if (pmin < sle) {
      var <- rownames(a)[a[,6] == pmin];
      if (length(var) > 1) {
        var <- var[2]
      }
      current <- update(current, paste("~ . + ", var), evaluate=FALSE)
      current <- eval.parent(current)
      if (verbose) {
        cat("\nStep:  \n", cut.string(deparse(as.vector(formula(current)))), "\n\n", sep = "")
        utils::flush.console()
      }
      next
    }
    break
  }
  return(current)
}
