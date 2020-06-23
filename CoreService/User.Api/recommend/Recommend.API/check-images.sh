if [ $(docker ps -a --format {{.Names}} | grep recommendapi) ]
then
    docker rm -f recommendapi
    docker rmi recommendapi
fi
