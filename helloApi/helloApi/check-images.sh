if [ $(docker ps -a --format {{.Names}} | grep helloApi) ]
then
    docker rm -f helloApi
    docker rmi helloApi
fi
