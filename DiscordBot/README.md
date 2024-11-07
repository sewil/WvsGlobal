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
- Combine the last three steps: `make deploy`