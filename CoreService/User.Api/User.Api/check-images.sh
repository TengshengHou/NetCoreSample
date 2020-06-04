if [ $(docker ps -a --format {{.Names}} | grep userapi) ]
then
    docker rm -f userapi
    docker rmi userapi
fi
