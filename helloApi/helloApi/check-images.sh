if [ $(docker ps -a --format {{.Names}} | grep helloapi) ]
then
    docker rm -f helloapi
    docker rmi helloapi
fi
