pipeline {
  agent any
  stages {
    stage('initialize') {
      steps {
        echo 'start'
      }
    }
    stage('clone code') {
      steps {
　　　　cd C:\Users\d0613\OneDrive\Documents\GitHub\MheOperator-JenkinsTest
       git pull https://github.com/HironobuTozuka/MheOperator-JenkinsTest.git
      }
    }

    stage('build') {
      steps {
        bat "\"${MSBUILD}\" MheOperator.sln /p:Configuration=${env.CONFIG};Platform=${env.PLATFORM} /maxcpucount:%NUMBER_OF_PROCESSORS% /nodeReuse:false"
      }
    }

  }
  environment {
    MSBUILD = 'C:\\Program Files (x86)\\Microsoft Visual Studio\\2019\\Community\\MSBuild\\Current\\Bin\\MSBuild.exe'
    CONFIG = 'Release'
    PLATFORM = 'x64'
  }
}
