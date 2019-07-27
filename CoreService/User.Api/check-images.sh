if [ $(docker ps -a --format {{.Names}} | grep user-api) ]
then
    docker rm -f user-api
    docker rmi -f user-api
fi