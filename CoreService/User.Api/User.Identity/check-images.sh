if [ $(docker ps -a --format {{.Names}} | grep useridentity) ]
then
    docker rm -f useridentity
    docker rmi useridentity
fi
