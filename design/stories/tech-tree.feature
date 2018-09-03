Scenario: Successful activation of tech tree entry
    Given Tech Tree is active
    When Player clicks the Sawyer icon
    And Resource requirements are met
    Then Sawyer icon in Tech Tree is marked complete and unclickable
    And Sawyer becomes available in Building Selection
    And Sawyer job is added to Job List
    And Resources are consumed
