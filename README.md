# QuickFixOrder

## COM DOCKER
Primeiro para rodar o serviço OrderAccumulator:
  - Entrar dentro da solução: cd QuickFixOrder\OrderAccumulatorConsole\OrderAccumulator
  - docker build -t order-accumulator-image -f Dockerfile .
  - docker run --name orderaccumulator -p 5001:5001 -it order-accumulator-image

Segundo para rodar o serviço OrderGenerator:
  - Entrar dentro da solução: cd QuickFixOrder\OrderGeneratorConsole\OrderGenerator
  - docker build -t order-generator-image -f Dockerfile .
  - docker run --network host -it order-generator-image

## SEM DOCKER
Para rodar a aplicação (caso você tenha o sdk 7.0), basta entrar dentro dos dois serviços e:

Primeiro para rodar o serviço OrderAccumulator:
  - cd QuickFixOrder\OrderAccumulatorConsole\OrderAccumulator
  - dotnet run

Segundo para rodar o serviço OrderGenerator:
  - cd QuickFixOrder\OrderGeneratorConsole\OrderGenerator
  - dotnet run
