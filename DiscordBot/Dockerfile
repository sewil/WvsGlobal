# Use an official Python runtime as a parent image
FROM python:3.8-slim

# Set the working directory in the container
WORKDIR /usr/src/app

# Copy the current directory contents into the container
COPY . .

# Copy the .env file into the container
COPY .env .env

# Install the necessary dependencies
RUN pip install -r requirements.txt

# Expose any ports the app runs on (not required for this bot)
EXPOSE 8080

# Run bot.py when the container launches
CMD ["python", "bot.py"]
