pipeline {
    agent any
    stages {
        stage('Checkout Code') {
            steps {
                git branch: 'main', url: 'https://github.com/sakibh20/secure-post.git'
            }
        }
        stage('Build') {
            steps {
                sh 'echo "Building Application..."'
            }
        }
        stage('Test') {
            steps {
                sh 'echo "Running Tests..."'
            }
        }
        stage('Deploy') {
            steps {
                sh 'echo "Deploying Application..."'
            }
        }
    }
}
