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
        bat "\"${TARGET_DIR}${CLONE_BAT}\""
      }
    }

    stage('build') {
      steps {
        bat "\"${MSBUILD}\" \"${TARGET_DIR}${TARGET_PRJ}\" /p:Configuration=${env.CONFIG};Platform=\"${env.PLATFORM}\" /maxcpucount:%NUMBER_OF_PROCESSORS% /nodeReuse:false"
      }
    }

  }
  environment {
    MSBUILD = 'C:\\Program Files (x86)\\Microsoft Visual Studio\\2019\\Community\\MSBuild\\Current\\Bin\\MSBuild.exe'
    TARGET_DIR = 'C:\\Users\\d0613\\OneDrive\\Documents\\GitHub\\MheOperator-JenkinsTest\\'
    TARGET_PRJ = 'MheOperator.sln'
    CLONE_BAT = 'gitpull.bat'
    CONFIG = 'Release'
    PLATFORM = 'Any CPU'
  }
}
