pipeline {
  agent any
  environment {
    MSBUILD = "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\"
    CONFIG = 'Release'
    PLATFORM = 'x64'
  }
  stages {
    stage('initialize') {
      steps {
        echo 'start'
      }
    }

    stage('build') {
      steps {
        bat "NuGet.exe restore your_project.sln"
        bat "\"${MSBUILD}\" your_project.sln /p:Configuration=${env.CONFIG};Platform=${env.PLATFORM} /maxcpucount:%NUMBER_OF_PROCESSORS% /nodeReuse:false"
      }
    }

  }
}
