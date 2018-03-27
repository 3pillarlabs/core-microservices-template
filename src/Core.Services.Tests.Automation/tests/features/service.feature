Feature: CoreService's automation tests

 Scenario: Get the version
    When I GET the version
    Then the http status should be 200
	
Scenario: Get the employees detail
    Given I have an authorized key
    And path parameter id = "100"
    When I send a "GET" request on "v1/{id}"
    Then the http status should be 200   
    And the result should not equal to null

Scenario: Get the employees list
    Given I have an authorized key   
    When I send a "GET" request on "v1/"
    Then the http status should be 200   
    And the result should equal to null

Scenario: Submit delete employee
    Given I have an authorized key
    And path parameter id = "105"
    When I send a "DELETE" request on "v1/{id}"
    Then the http status should be 200	

Scenario: Submit Add new employee
    Given I have an authorized key
    And body parameter Name = "Automation User1"
    And body parameter Address = "Delhi"
    And body parameter DepartmentId = "1002"
    And body parameter Salary = "3500"
    When I send a "POST" request on "v1/"
    Then the http status should be 200
	And the success should equal "true"