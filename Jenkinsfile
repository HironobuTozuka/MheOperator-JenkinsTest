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
        bat "\"${DOTNET}\" clean \"${TARGET_PRJ}\""
        bat "\"${DOTNET}\" build \"${TARGET_PRJ}\""
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
    DOTNET = 'C:\\Program Files\\dotnet\\dotnet'
  }
}
