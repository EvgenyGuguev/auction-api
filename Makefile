up: docker-dev-up
stop: docker-dev-stop
restart: stop up

docker-dev-up:
	docker-compose -f docker-compose-dev.yml up -d
	
docker-dev-build:
	docker-compose -f docker-compose-dev.yml build
	
docker-dev-stop:
	docker-compose -f docker-compose-dev.yml stop
