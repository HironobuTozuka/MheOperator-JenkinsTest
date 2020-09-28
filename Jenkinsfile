pipeline {
  agent any
  stages {
    stage('initialize') {
      steps {
        bat 'echo %~dp0'
      }
    }

    stage('build') {
      steps {
        bat "donet clean \"${TARGET_PRJ}\""
        bat "donet build \"${TARGET_PRJ}\""
      }
    }

    stage('unit test') {
      steps {
        echo 'unitTest'
      }
    }

    stage('deploy') {
      steps {
        echo 'deploy'
      }
    }

  }
  environment {
    TARGET_PRJ = 'C:\\Jenkins\\workspace\\MheOperator-JenkinsTest_master\\MheOperator.sln'
  }
}
