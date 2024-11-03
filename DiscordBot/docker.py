from dotenv import load_dotenv
import os
import sys
import subprocess

# Load environment variables from .env file
load_dotenv()

DOCKER_IMAGE = os.getenv("DOCKER_IMAGE")
ECR_REPO_URI = os.getenv("AWS_ECR_REPO_URI")
REGION = os.getenv('AWS_REGION')
ACCOUNT_ID = os.getenv('AWS_ACCOUNT_ID')

if len(sys.argv) == 0:
  print('No arguments!')
elif sys.argv[1] == 'auth':
  subprocess.run(['aws', 'ecr', 'get-login-password', '--region', REGION, '|', 'docker', 'login', '--username', 'AWS', '--password-stdin', f'{ACCOUNT_ID}.dkr.ecr.{REGION}.amazonaws.com'])
elif sys.argv[1] == 'build':
  subprocess.run(['docker', 'build', '--platform', 'linux/amd64', '-t', DOCKER_IMAGE, '.'])
elif sys.argv[1] == 'run':
  subprocess.run(['docker', 'run', '--name', DOCKER_IMAGE, '--env-file', '.env', DOCKER_IMAGE])
elif sys.argv[1] == 'tag':
  subprocess.run(['docker', 'tag', f'{DOCKER_IMAGE}:latest', ECR_REPO_URI])
elif sys.argv[1] == 'push':
  subprocess.run(['docker', 'push', ECR_REPO_URI])
else:
   print('Unknown argument!')
