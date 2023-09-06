# CI / CD to the dangerzone
In this repository we create a showcase how you can inject external [.LUA scripts](https://www.moonsharp.org/) into your already build and
bundled game. For that we verify the files in our CI / CD with a private / public key pair. Then we drop the .LUA file onto a Cloud bucket of Firebase and the corresponding Signature file.

In our Unity project we receive the signature and .lua file and verify that the file and the signature match with the embedded public key. This allows us to execute the .lua file and be sure that the code has not been altered in the meantime. 

## Special Thanks
[CI-CD-Dangerzone](https://github.com/codemagic-ci-cd/ci_to_dangerzone) by [maks](https://github.com/maks)

## This repository is supported and sponsored by [Codemagic](https://codemagic.io/)
<img src="https://media.githubusercontent.com/media/Flutter-Explained/ci_to_dangerzone_unity/main/images/codemagic.png"/>
