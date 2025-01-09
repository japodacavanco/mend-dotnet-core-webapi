param(
	[Parameter(Mandatory=$true)]
	[string]$directoryPath,
	[ValidateSet("SCA", "SAST")]
	[Parameter(Mandatory=$true)]
	[string]$scanType,
	[ValidateSet("BaseLine","Incremental")]
	[Parameter(Mandatory=$false)]
	[string]$sastType="BaseLine"
)

try {
	switch ($scanType) {
		"SCA" {
			mend sca -d $directoryPath -s "POC_APP_MEND//POC_PROJ_API_01" -u --label-app "POC-App" --label-proj "POC-Proj"
		}
		"SAST" {
			if($sastType -eq "BaseLine") {
				mend sast -d $directoryPath -s "POC_APP_MEND//POC_PROJ_API_01" --label-app "POC-App" --label-proj "POC-Proj" --upload-baseline
			}
			else {
				mend sast -d $directoryPath -s "POC_APP_MEND//POC_PROJ_API_01" --label-app "POC-App" --label-proj "POC-Proj" --inc
			}
		}
	}
}
catch {
	Write-Host "MEND CLI scan failed: $_"
	exit 1
}
