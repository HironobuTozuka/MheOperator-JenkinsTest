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
        bat "\"${NUGET}\" restore \"${TARGET_PRJ}\""
        bat "\"${MSBUILD}\" \"${TARGET_PRJ}\" /t:restore /p:Configuration=${env.CONFIG};Platform=\"${env.PLATFORM}\" /maxcpucount:%NUMBER_OF_PROCESSORS% /nodeReuse:false"
        bat "\"${MSBUILD}\" \"${TARGET_PRJ}\" /t:clean /p:Configuration=${env.CONFIG};Platform=\"${env.PLATFORM}\" /maxcpucount:%NUMBER_OF_PROCESSORS% /nodeReuse:false"
        bat "\"${MSBUILD}\" \"${TARGET_PRJ}\" /t:build /p:Configuration=${env.CONFIG};Platform=\"${env.PLATFORM}\" /maxcpucount:%NUMBER_OF_PROCESSORS% /nodeReuse:false"
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
    MSBUILD = 'C:\\Program Files (x86)\\Microsoft Visual Studio\\2019\\Community\\MSBuild\\Current\\Bin\\MSBuild.exe'
    NUGET = 'C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\Enterprise\\MSBuild\\ReadyRoll\\OctoPack\\build\\NuGet.exe'
    TARGET_PRJ = 'C:\\Jenkins\\workspace\\MheOperator-JenkinsTest_master\\MheOperator.sln'
    CONFIG = 'Release'
    PLATFORM = 'Any CPU'
  }
}
