pipeline {
    agent any
    triggers {
        cron '* * * * *'
    }
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
