FROM chemsorly/msbuilder:1.1.1
SHELL ["powershell"]

COPY . 'C:\\build\\'  
WORKDIR 'C:\\build\\'

ARG MSBUILD_PROJECT=""
ARG MSBUILD_TARGET=""
ARG MSBUILD_ARGS=""

RUN Import-PfxCertificate -FilePath .\VisioAddin2013\VisioAddin2013_TemporaryKey.pfx -CertStoreLocation Cert:\CurrentUser\My -Verbose
RUN ["nuget.exe", "restore"]  
RUN & 'C:\\Program Files (x86)\\MSBuild\\14.0\\Bin\\msbuild.exe' $env:MSBUILD_PROJECT $env:MSBUILD_TARGET $env:MSBUILD_ARGS