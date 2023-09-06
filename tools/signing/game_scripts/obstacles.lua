-- this function needs to be named exactly "doObstacles" to be called by host Dart env
function doObstacles()
  -- mkObstacle is defined by the host Dart env
  mkObstacle(10, -1);
  mkObstacle(15, 7);
  mkObstacle(2, 7);
  mkObstacle(-16, 2);
end

function doWeather()
  mkWeather(0)
end

function doDayNight()
  mkDayNight(1);
end