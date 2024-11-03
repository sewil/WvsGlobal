## Setting up
1. Log in to the AWS Management Console.
2. Navigate to the Elastic Container Registry (ECR) service.
3. Create a New Repository:
    - Go to “Repositories” > “Create Repository.”
    - Provide a name for your repository (e.g., my-docker-image).
    - Choose other settings as needed, then click Create Repository.
    - Once created, AWS will provide you with the repository URI, which will look something like this: 123456789012.dkr.ecr.us-east-1.amazonaws.com/my-docker-image.

## Deploying
1.	Install the AWS CLI if it’s not already installed:
```
pip install awscli
```

2.	Configure AWS CLI with your access credentials:
```
aws configure
```

3. Authenticate Docker to AWS ECR:
```
python docker.py auth
```

4. Tag the Docker Image for ECR:
```
python docker.py tag
```

5. Push the Docker Image to ECR:
```
python docker.py push
```
