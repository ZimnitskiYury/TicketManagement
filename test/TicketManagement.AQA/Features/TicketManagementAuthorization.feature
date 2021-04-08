Feature: TicketManagementAuthorization
	As a unauthorized user
	I want to be able to login on site
	So I can do it through home page header

	@succesful
Scenario: Successful login to the site
	Given User is on localhost
	When User press link Login in header
	And Enters "Event" to Login input form
	And Enters "AA11aa_" to Password input form
	And User clicks button LoginSubmit
	Then Header text change to Login "Event"

	
Scenario: Failed login to the site
	Given User is on localhost
	When User press link Login in header
	And Enters "NotEvent" to Login input form
	And Enters "112gAA11aa_" to Password input form
	And User clicks button LoginSubmit
	Then Login form has error "Неудачная попытка входа."
