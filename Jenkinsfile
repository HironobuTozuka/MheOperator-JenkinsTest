pipeline {
  agent any
  stages {
    stage('initialize') {
      steps {
        echo 'start'
      }
    }

    stage('clone') {
      steps {
        echo 'test'
      }
    }

    stage('build') {
      steps {
        bat '"\\"${MSBUILD}\\" ${TARGET} /p:Configuration=${env.CONFIG};Platform=\\"${env.PLATFORM}\\" /maxcpucount:%NUMBER_OF_PROCESSORS% /nodeReuse:false"'
      }
    }

  }
  environment {
    MSBUILD = 'C:\\Program Files (x86)\\Microsoft Visual Studio\\2019\\Community\\MSBuild\\Current\\Bin\\MSBuild.exe'
    TARGET = 'C:\\Users\\d0613\\OneDrive\\Documents\\GitHub\\MheOperator-JenkinsTest\\MheOperator.sln'
    CONFIG = 'Release'
    PLATFORM = 'Any CPU'
  }
}