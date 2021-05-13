up: docker-dev-up
stop: docker-dev-stop
restart: stop up

docker-dev-up:
	docker-compose -f docker-compose-dev up -d
	
docker-dev-build:
	docker-compose -f docker-compose-dev build
	
docker-dev-stop:
	docker-compose -f docker-compose-dev stop
