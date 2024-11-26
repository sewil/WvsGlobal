## Setting up
1. Log in to the AWS Management Console.
2. Navigate to the Elastic Container Registry (ECR) service.
3. Create a New Repository:
    - Go to “Repositories” > “Create Repository.”
    - Provide a name for your repository (e.g., my-docker-image).
    - Choose other settings as needed, then click Create Repository.
    - Once created, AWS will provide you with the repository URI, which will look something like this: 123456789012.dkr.ecr.us-east-1.amazonaws.com/my-docker-image.

## Deploying
- Install the AWS CLI if it’s not already installed: `pip install awscli`
  - (Windows) Install Make: `choco install make`
- Configure AWS CLI with your access credentials: `aws configure`
- Authenticate Docker to AWS ECR: `make auth`
- Build the Docker Image: `make build`
- Tag the Docker Image for ECR: `make tag`
- Push the Docker Image to ECR: `make push`

### Deploying to ECS Cluster
After making changes and pushing them to the ECR, update the service:
```
aws ecs update-service --cluster $(AWS_CLUSTER_NAME) --service $(AWS_SERVICE) --force-new-deployment --region $(AWS_REGION)
```

### Deploying to EC2
- Connect to your instance: `ssh -i $(AWS_PRIVATE_KEY_FILE) $(AWS_EC2_HOST_URL)`
- Install Docker and set permissions:
  ```
  sudo yum update -y
  sudo yum install -y docker
  sudo service docker start
  ```
- Set Docker permissions (restart after to apply changes): `sudo usermod -a -G docker $(AWS_EC2_USER)`
- Authenticate with AWS:
  ```
  aws configure
  aws ecr get-login-password --region $(AWS_REGION) | docker login --username AWS --password-stdin $(AWS_ECR_REGISTRY)
  ```
- Pull and run the image:
  ```
  docker pull $(AWS_ECR_REPO_URI):latest 
  docker run -d -p 80:80 $(AWS_ECR_REPO_URI):latest
  ```
