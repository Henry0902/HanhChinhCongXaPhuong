# Hướng dẫn cập nhật hệ thống

## Thay đổi dropdown lstCanBoXuLys để chỉ hiển thị cán bộ xử lý

### Các thay đổi đã thực hiện:

1. **Thêm constant cho Role ID** (`DNC.CM/CommonConstants.cs`):
   - Thêm `ROLE_CANBO_XULY = 2016` để lưu trữ Role ID của cán bộ xử lý

2. **Thêm method mới trong UserController** (`DNC.WEB/Controllers/UserController.cs`):
   - `GetAllByRole(int roleId)`: Lấy user theo role ID
   - `GetCanBoXuLy()`: Lấy cán bộ xử lý sử dụng constant

3. **Thêm method mới trong UsersRp** (`DNC.WEB/Repository/UsersRp.cs`):
   - `GetUsersByRole(int roleId)`: Sử dụng stored procedure `Users_GetByRole` để lấy user theo role

4. **Cập nhật JavaScript** (`DNC.WEB/ScriptController/DonThuTiepNhanJs.js`):
   - Thay đổi function `getAllCanBoXuLy()` để sử dụng method `GetCanBoXuLy()`
   - Thêm trường `IdCanBoXuLy` vào object `HuongXuLy`
   - Thêm validation cho trường `IdCanBoXuLy`
   - Cập nhật function `createHuongXuLy()` để gửi `IdCanBoXuLy` lên server

5. **Cập nhật Controller và Repository**:
   - Thêm tham số `IdCanBoXuLy` vào method `UpdateHuongXuLy` trong `DonThuController`
   - Cập nhật method `UpdateHuongXuLy` trong `DonThuRp` để nhận thêm tham số

### Cần thực hiện thêm:

1. **Tạo stored procedure `Users_GetByRole`**:
   ```sql
   CREATE PROCEDURE Users_GetByRole
       @RoleId INT
   AS
   BEGIN
       SELECT 
           u.Id,
           u.UserName,
           u.DisplayName,
           u.DisplayId,
           u.Mobile,
           u.Email,
           u.Position,
           u.CreatedDate,
           u.IsLocked,
           u.IsDeleted,
           u.DepartmentId
       FROM Users u
       INNER JOIN UsersRoles ur ON u.Id = ur.UserId
       WHERE ur.RoleId = @RoleId
           AND u.IsDeleted = 0
           AND u.IsLocked = 0
       ORDER BY u.DisplayName
   END
   ```

2. **Cập nhật stored procedure `DonThu_UpdateHuongXuLy`**:
   ```sql
   -- Thêm tham số @IdCanBoXuLy vào stored procedure
   -- Cập nhật logic để lưu trữ thông tin cán bộ xử lý
   ```

### Lưu ý:
- Dropdown sẽ chỉ hiển thị các user có role "cán bộ xử lý" (Role ID = 2016)
- Thông tin cán bộ xử lý sẽ được lưu trữ khi cập nhật hướng xử lý
- Cần validation để đảm bảo người dùng chọn cán bộ xử lý trước khi lưu
- Sử dụng stored procedure `Users_GetByRole` để tối ưu hiệu suất truy vấn
