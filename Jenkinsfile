pipeline {
    agent any
    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }
        stage('Print a message') {
            steps {
                echo 'Hello world!' 
            }
        }
    }
}
