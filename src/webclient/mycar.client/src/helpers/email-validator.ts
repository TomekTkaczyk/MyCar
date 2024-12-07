const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

function isValidEmail(email: string): boolean {
  return emailRegex.test(email);
}

const email = 'example@email.com';
if (isValidEmail(email)) {
  console.log('Valid email address');
} else {
  console.log('Invalid email address');
}

export {isValidEmail};
