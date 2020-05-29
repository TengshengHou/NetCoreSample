if [ $(docker ps -a --format {{.Names}} | grep gateway) ]
then
    docker rm -f gateway
    docker rmi gateway
fi
