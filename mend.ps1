param(
	# Accept specific values in a parameter
	[ValidateSet("SCA", "SAST")]
	[Parameter(Mandatory=$true)]
	[string]$scanType
)

try {
	switch ($scanType) {
		"SCA" {
			mend sca -d . -s "POC_APP_MEND//POC_PROJ_API_01" -u --label-app "POC-App" --label-proj "POC-Proj"
		}
		"SAST" {
			mend sast -d . -s "POC_APP_MEND//POC_PROJ_API_01" --label-app "POC-App" --label-proj "POC-Proj"
		}
	}
}
catch {
	Write-Host "MEND CLI scan failed: $_"
	exit 1
}
