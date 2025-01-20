export default interface userDto {
  id: string,
  name: string,
  firstName: string,
  lastName: string,
  claims: string[],
  email: string,
  emailConfirm: boolean,
  isActive: boolean,
  role: string
}
