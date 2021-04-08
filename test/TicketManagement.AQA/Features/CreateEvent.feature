Feature: CreateEvent
	As a EventManager
	I want to create new event with prices
	So i do it from events page

@mytag
Scenario: Successfully created new event
	Given User is on localhost
	When User press link Login in header
	And Enters "Event" to Login input form
	And Enters "AA11aa_" to Password input form
	And User clicks button LoginSubmit
	And User clicks on Events page
	And User clicks Create button
	And Enters "First event" to Name input form
	And Enters "Event description Large" to Description input form
	And Enters "1" to Layout input form
	And Enters "19.10.2022 14:11" to Start Event Date input form
	And Enters "20.10.2022 14:11" to End Event Date input form
	And User clicks button Submit to Create Event
	And Enters "200" to First Area Price input form
	And Enters "300" to Second Area Price input form
	And User clicks button Submit to Change Prices
	Then A new event will appear on the page with the name "First event"