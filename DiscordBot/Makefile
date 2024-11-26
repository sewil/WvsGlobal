include .env

auth:
	aws ecr get-login-password --region $(AWS_REGION) | docker login --username AWS --password-stdin $(AWS_ECR_REGISTRY)
build:
	docker build -t $(DOCKER_IMAGE) .
run:
	docker run --name $(DOCKER_IMAGE) --env-file .env $(DOCKER_IMAGE)
tag:
	docker tag $(DOCKER_IMAGE):latest $(AWS_ECR_REPO_URI)
push:
	docker push $(AWS_ECR_REPO_URI)
deploy:
	make build
	make tag
	make push
