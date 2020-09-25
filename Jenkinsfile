pipeline {
  agent any
  stages {
    stage('initialize') {
      steps {
        bat 'echo %~dp0'
      }
    }

    stage('clone') {
      steps {
        bat 'echo %~dp0'
      }
    }

    stage('build') {
      steps {
        bat 'echo %~dp0'
        bat 'cd ..'
        bat 'echo %~dp0'
        bat 'cd ..'
        bat 'echo %~dp0'
        bat 'cd MheOperator-JenkinsTest_master'
        bat 'echo %~dp0'
        bat "\"${MSBUILD}\" \"${TARGET_PRJ}\" /p:Configuration=${env.CONFIG};Platform=\"${env.PLATFORM}\" /maxcpucount:%NUMBER_OF_PROCESSORS% /nodeReuse:false"
      }
    }

  }
  environment {
    MSBUILD = 'C:\\Program Files (x86)\\Microsoft Visual Studio\\2019\\Community\\MSBuild\\Current\\Bin\\MSBuild.exe'
    TARGET_PRJ = 'MheOperator.sln'
    CONFIG = 'Release'
    PLATFORM = 'Any CPU'
  }
}