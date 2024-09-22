# General Guidelines
- Focus on Key Requirements: Ensure that you address the core requirements of each exercise effectively. While depth and thoroughness are important, be mindful of focusing on the essential aspects of the task and delivering a functional solution.
- Demonstrate Best Practices: Use best practices in coding, design, and security. For frontend development, this includes clean and maintainable code, responsive design, and user experience considerations. For backend development, ensure robustness, scalability, and secure handling of data.
- Provide Documentation: Include brief documentation explaining how to set up, run, and test your solution. This should cover any setup instructions, dependencies, and an overview of your implementation.

## Exercise 1: Message List Component
Exercise Overview: In this part, you will create a message list component for a chat window that supports multiple message types. The component should display a list of messages, including text messages, images, charts, and tables. Additionally, you need to include an input box where users can add their own messages formatted in markdown.

### Requirements:

- The component should display a list of messages.
- Each message can be one of the following types:
	- Text message: Display text formatted in markdown.
	- Image: Display an image.
	- Chart: Display a chart.
	- Table: Display a table.
- Include an input box where users can add their own messages to the chat. This input box should allow users to enter text messages formatted in markdown.

## Exercise 2: Backend API
Exercise Overview: In this part, you will create a backend API to support the message list component. The API should handle fetching, adding, and deleting messages.

 ### Requirements:

- Create an API with endpoints to:
- Fetch all messages.
- Add a new message.
- Delete a message by ID.
- Use a database to store the messages. You can use any database of your choice (e.g., MongoDB, PostgreSQL).
- Implement error handling for the API to manage scenarios such as database connection failures or invalid input.

## Exercise 3: Authentication and Authorization
Exercise Overview: In this part, you will implement authentication and authorization for the message list component and the backend API.

### Requirements:

- - Implement user authentication using a method of your choice (e.g., JWT, OAuth).
- Add authorization to ensure that only authenticated users can add or delete messages.
- Protect the API endpoints to ensure that only authorized users can access them.