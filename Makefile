# QUALITY

test:
	dotnet test

mutation.install-tool:
	dotnet tool install --global dotnet-stryker

mutation.business:
	cd ./DriverLicenseApplication.Application.Tests && dotnet stryker -o

mutation.adapters:
	cd ./DriverLicenseApplication.Adapters.Tests && dotnet stryker -o

mutation: mutation.business mutation.adapters

quality: test mutation

# CLEAN

clean:
	dotnet clean
	rm -rf **/StrykerOutput
