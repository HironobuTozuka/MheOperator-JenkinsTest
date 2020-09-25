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
        bat "cd ${GIT_DIR}"
        bat "rmdir /s /q ${TARGET_DIR}"
        bat "git clone https://github.com/HironobuTozuka/MheOperator-JenkinsTest.git"
      }
    }

    stage('build') {
      steps {
        bat "\"${MSBUILD}\" \"${GIT_DIR}${TARGET_PRJ}\" /p:Configuration=${env.CONFIG};Platform=\"${env.PLATFORM}\" /maxcpucount:%NUMBER_OF_PROCESSORS% /nodeReuse:false"
      }
    }

  }
  environment {
    MSBUILD = 'C:\\Program Files (x86)\\Microsoft Visual Studio\\2019\\Community\\MSBuild\\Current\\Bin\\MSBuild.exe'
    GIT_DIR = 'C:\\Users\\d0613\\OneDrive\\Documents\\GitHub\\'
    TARGET_DIR = 'MheOperator-JenkinsTest\\'
    TARGET_PRJ = 'MheOperator.sln'
    CLONE_BAT = 'clone.bat'
    CONFIG = 'Release'
    PLATFORM = 'Any CPU'
  }
}
