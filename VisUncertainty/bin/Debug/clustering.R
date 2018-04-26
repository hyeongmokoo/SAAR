Bhatta.mdist <- function(x1, x2, v1, v2) ##No covariance 
{
  bha.dist <- bhattacharyya.dist(x1, x2, diag(as.vector(v1)), diag(as.vector(v2)))
  return(bha.dist)
}